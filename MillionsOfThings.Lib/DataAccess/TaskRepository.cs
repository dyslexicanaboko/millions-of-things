using Dapper;
using MillionsOfThings.Lib.DataAccess.Utility;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Services;
using MillionsOfThings.Lib.Services.Utility;
using Npgsql;
using System.Data;
using System.Text;
using static Dapper.SqlMapper;

namespace MillionsOfThings.Lib.DataAccess
{
  public class TaskRepository
		: BaseRepository, ITaskRepository
  {
    private static readonly List<ColumnSchema> UpdatePartialColumns = new()
    {
      new ColumnSchema(nameof(TaskEntity.CategoryId), "category_id", DbType.Int32),
      new ColumnSchema(nameof(TaskEntity.Description), "description", DbType.String, 255),
      new ColumnSchema(nameof(TaskEntity.IsFinished), "is_finished", DbType.Boolean),
      new ColumnSchema(nameof(TaskEntity.FinishedOn), "finished_on", DbType.DateTime2, scale: 0)
    };

    public TaskRepository(IAppConfiguration configuration)
			: base(configuration)
		{
		}

		public async Task<IEnumerable<TaskEntity>> SelectAll(int userId)
		{
			const string sql = @"
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
			WHERE user_id = @user_id";

			await using var connection = new NpgsqlConnection(ConnectionString);

			return (await connection.QueryAsync<TaskEntity>(sql, new { UserId = userId })).ToList();
		}

    public async Task<TaskEntity?> Select(int taskId)
    {
      const string sql = @"
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
			WHERE task_id = @task_id";

      await using var connection = new NpgsqlConnection(ConnectionString);

      var lst = (await connection.QueryAsync<TaskEntity>(sql, GetPrimaryKeyParameter(taskId))).ToList();

      return lst.SingleOrDefault();
    }

    public async Task<IEnumerable<TaskEntity>> SelectAll()
    {
      const string sql = @"
			SELECT
	                task_id,
                user_id,
                category_id,
                description,
                is_finished,
                finished_on,
                created_on,
                modified_on
			FROM public.task";

      await using var connection = new NpgsqlConnection(ConnectionString);

      return (await connection.QueryAsync<TaskEntity>(sql)).ToList();
    }

    public async Task<int> Insert(TaskEntity entity)
    {
      const string sql = @"INSERT INTO public.task (
                user_id,
                category_id,
                description
						) VALUES (
                @user_id,
                @category_id,
                @description)	RETURNING task_id AS PK;";

      await using var connection = new NpgsqlConnection(ConnectionString);

      var p = new DynamicParameters();
      p.Add(name: "@user_id", dbType: DbType.Int32, value: entity.UserId);
      p.Add(name: "@category_id", dbType: DbType.Int32, value: entity.CategoryId);
      p.Add(name: "@description", dbType: DbType.String, value: entity.Description, size: 255);

      return await connection.ExecuteScalarAsync<int>(sql, p);
    }

    public async Task Update(TaskEntity entity)
    {
      const string sql = @"UPDATE public.task SET 
                category_id = @category_id,
                description = @description,
                is_finished = @is_finished,
                finished_on = @finished_on,
                modified_on = now()
						WHERE task_id = @task_id";

      await using var connection = new NpgsqlConnection(ConnectionString);

      var p = new DynamicParameters();
      p.Add(name: "@task_id", dbType: DbType.Int32, value: entity.TaskId);
      p.Add(name: "@category_id", dbType: DbType.Int32, value: entity.CategoryId);
      p.Add(name: "@description", dbType: DbType.String, value: entity.Description, size: 255);
      p.Add(name: "@is_finished", dbType: DbType.Boolean, value: entity.IsFinished);
      p.Add(name: "@finished_on", dbType: DbType.DateTime2, value: entity.FinishedOn, scale: 0);

      await connection.ExecuteAsync(sql, p);
    }

    public async Task UpdatePartial(int taskId, IList<UpdateInstruction> instructions)
    {
      if (!instructions.Any()) return;

      var p = new DynamicParameters();
      p.Add(name: "@task_id", dbType: DbType.Int32, value: taskId);

      var lst = new List<string>(instructions.Count);

      foreach (var instr in instructions)
      {
        var col = UpdatePartialColumns.SingleOrDefault(x => x.Property == instr.Property);
        
        if (col == null) throw new ArgumentException($"The property '{instr.Property}' is not valid for partial updates.", nameof(instructions));

        lst.Add($"{col.Name} = @{col.Name}");

        p.Add(name: col.Name, dbType: col.DbType, value: instr.Value, size: col.Size, scale: col.Scale);
      }

      await using var connection = new NpgsqlConnection(ConnectionString);

      const string template = @"UPDATE public.task SET {0}, modified_on = now() WHERE task_id = @task_id";
      
      var sql = string.Format(template, string.Join(", ", lst));

      await connection.ExecuteAsync(sql, p);
    }

    public async Task Delete(int taskId)
    {
      const string sql = "DELETE FROM public.task WHERE task_id = @task_id";

      await using var connection = new NpgsqlConnection(ConnectionString);

      await connection.ExecuteAsync(sql, GetPrimaryKeyParameter(taskId));
    }

    private DynamicParameters GetPrimaryKeyParameter(int taskId)
    {
      var p = new DynamicParameters();
      p.Add(name: "@task_id", dbType: DbType.Int32, value: taskId);

      return p;
    }
  }
}
