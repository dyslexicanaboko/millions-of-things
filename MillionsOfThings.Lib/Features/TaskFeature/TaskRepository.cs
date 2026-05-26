using Dapper;
using MillionsOfThings.Lib.Utility;
using System.Data;

namespace MillionsOfThings.Lib.Features.TaskFeature
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

    public async Task<List<TaskRecord>> ReadAll(int userId)
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

      await using var connection = await GetConnection();

      return (await connection.QueryAsync<TaskRecord>(sql, new { UserId = userId })).ToList();
    }

    public async Task<TaskRecord?> Read(int taskId, int userId)
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

      await using var connection = await GetConnection();

      var p = GetPrimaryKeyParameter(taskId);
      AddUserIdParameter(p, userId);

      return (await connection.QueryAsync<TaskRecord>(sql, p)).SingleOrDefault();
    }

    public async Task<List<TaskRecord>> ReadAll()
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

      await using var connection = await GetConnection();

      return (await connection.QueryAsync<TaskRecord>(sql)).ToList();
    }

    public async Task<int> Create(TaskRecord entity)
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

      await using var connection = await GetConnection();

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

      return await connection.ExecuteScalarAsync<int>(sql, p);
    }

    //TODO: UserId is missing here why?
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

      await using var connection = await GetConnection();

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

      await connection.ExecuteAsync(sql, p);
    }

    public async Task UpdatePartial(int userId, int taskId, List<UpdateInstruction> instructions)
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

      await using var connection = await GetConnection();

      var p = GetPrimaryKeyParameter(taskId);
      AddUserIdParameter(p, userId);

      await connection.ExecuteAsync(sql, p);
    }

    private static DynamicParameters GetPrimaryKeyParameter(int taskId)
    {
      var p = new DynamicParameters();
      p.Add("@task_id", dbType: DbType.Int32, value: taskId);

      return p;
    }
  }
}
