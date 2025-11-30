using Dapper;
using MillionsOfThings.Lib.DataAccess.Utility;
using MillionsOfThings.Lib.Records;
using MillionsOfThings.Lib.Services;
using MillionsOfThings.Lib.Services.Utility;
using System.Data;

namespace MillionsOfThings.Lib.DataAccess
{
  public class TaskRepository
    : BaseRepository, ITaskRepository
  {
    private static readonly List<ColumnSchema> UpdatePartialColumns = new()
    {
      new ColumnSchema(nameof(TaskRecord.CategoryId), "category_id", DbType.Int32),
      new ColumnSchema(
        nameof(TaskRecord.Description),
        "description",
        DbType.String,
        255),
      new ColumnSchema(nameof(TaskRecord.IsFinished), "is_finished", DbType.Boolean),
      new ColumnSchema(
        nameof(TaskRecord.FinishedOn),
        "finished_on",
        DbType.DateTime2,
        scale: 0)
    };

    public TaskRepository(IAppConfiguration configuration)
      : base(configuration)
    {
    }

    public async Task<IList<TaskRecord>> SelectAll(int userId)
    {
      const string sql = """

                         			SELECT
                         	              task_id,
                                         user_id,
                                         category_id,
                                         description,
                                         is_finished,
                                         finished_on,
                                         created_on,
                                         modified_on
                         			FROM public.task
                         			WHERE user_id = @user_id
                         """;

      var connection = await GetConnection();

      return (await connection.QueryAsync<TaskRecord>(sql, new { UserId = userId })).ToList();
    }

    public async Task<TaskRecord?> Select(int taskId, int userId)
    {
      const string sql = """
                           SELECT
                             task_id,
                             user_id,
                             category_id,
                             description,
                             is_finished,
                             finished_on,
                             created_on,
                             modified_on
                           FROM public.task
                           WHERE task_id = @task_id 
                             AND user_id = @user_id
                         """;

      var connection = await GetConnection();

      var p = GetPrimaryKeyParameter(taskId);
      p.Add("@user_id", dbType: DbType.Int32, value: userId);

      var lst = (await connection.QueryAsync<TaskRecord>(sql, p)).ToList();

      return lst.SingleOrDefault();
    }

    public async Task<IList<TaskRecord>> SelectAll()
    {
      const string sql = """

                         			SELECT
                         	                task_id,
                                         user_id,
                                         category_id,
                                         description,
                                         is_finished,
                                         finished_on,
                                         created_on,
                                         modified_on
                         			FROM public.task
                         """;

      var connection = await GetConnection();

      return (await connection.QueryAsync<TaskRecord>(sql)).ToList();
    }

    public async Task<int> Insert(TaskRecord entity)
    {
      const string sql = """
                         INSERT INTO public.task (
                             user_id,
                             category_id,
                             description,
                             created_on
                         ) VALUES (
                             @user_id,
                             @category_id,
                             @description,
                             @created_on
                         ) RETURNING task_id AS PK;
                         """;

      var connection = await GetConnection();

      var p = new DynamicParameters();
      AddUserIdParameter(p, entity.UserId);

      p.Add("@category_id", dbType: DbType.Int32, value: entity.CategoryId);

      p.Add(
        "@description",
        dbType: DbType.String,
        value: entity.Description,
        size: 255);

      p.Add(
        "@created_on",
        dbType: DbType.DateTime2,
        value: entity.CreatedOn,
        scale: 0);

      return await connection.ExecuteScalarAsync<int>(sql, p, Transaction);
    }

    public async Task Update(TaskRecord entity)
    {
      const string sql = """
                         UPDATE public.task SET 
                           category_id = @category_id,
                           description = @description,
                           is_finished = @is_finished,
                           finished_on = @finished_on,
                           modified_on = now()
                         WHERE task_id = @task_id
                         """;

      var connection = await GetConnection();

      var p = new DynamicParameters();
      p.Add("@task_id", dbType: DbType.Int32, value: entity.TaskId);
      p.Add("@category_id", dbType: DbType.Int32, value: entity.CategoryId);

      p.Add(
        "@description",
        dbType: DbType.String,
        value: entity.Description,
        size: 255);

      p.Add("@is_finished", dbType: DbType.Boolean, value: entity.IsFinished);

      p.Add(
        "@finished_on",
        dbType: DbType.DateTime2,
        value: entity.FinishedOn,
        scale: 0);

      await connection.ExecuteAsync(sql, p, Transaction);
    }

    public async Task UpdatePartial(int userId, int taskId, IList<UpdateInstruction> instructions)
    {
      const string template = "UPDATE public.task SET {0}, modified_on = now() WHERE user_id = @user_id and task_id = @task_id";

      var p = GetPrimaryKeyParameter(taskId);
      AddUserIdParameter(p, userId);

      await UpdatePartial(
        template,
        UpdatePartialColumns,
        p,
        instructions);
    }

    public async Task Delete(int userId, int taskId)
    {
      const string sql = "DELETE FROM public.task WHERE user_id = @user_id and task_id = @task_id";

      var connection = await GetConnection();

      var p = GetPrimaryKeyParameter(taskId);
      AddUserIdParameter(p, userId);

      await connection.ExecuteAsync(sql, p, Transaction);
    }

    private static DynamicParameters GetPrimaryKeyParameter(int taskId)
    {
      var p = new DynamicParameters();
      p.Add("@task_id", dbType: DbType.Int32, value: taskId);

      return p;
    }
  }
}
