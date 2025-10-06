using Dapper;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Services;
using Npgsql;
using System.Data;

namespace MillionsOfThings.Lib.DataAccess
{
  public class CategoryRepository
    : BaseRepository, ICategoryRepository
  {
    public CategoryRepository(IAppConfiguration configuration)
      : base(configuration)
    {
    }

    public async Task<CategoryEntity?> Select(int userId, int categoryId)
    {
      const string sql = """

                         			SELECT
                         	      category_id,
                                user_id,
                                name,
                                created_on,
                                modified_on
                         			FROM public.category
                         			WHERE user_id = @user_id
                               AND category_id = @category_id 
                         """;

      await using var connection = new NpgsqlConnection(ConnectionString);

      var p = GetPrimaryKeyParameter(categoryId);
      AddUserIdParameter(p, userId);

      var lst = (await connection.QueryAsync<CategoryEntity>(sql, p)).ToList();

      return lst.SingleOrDefault();
    }

    public async Task<IEnumerable<CategoryEntity>> SelectAll()
    {
      const string sql = """
                         SELECT
                           category_id,
                           user_id,
                           name
                         FROM public.category
                         """;

      await using var connection = new NpgsqlConnection(ConnectionString);

      return await connection.QueryAsync<CategoryEntity>(sql);
    }

    public async Task<IEnumerable<CategoryEntity>> SelectAll(int userId)
    {
      const string sql = """
                         SELECT
                           category_id,
                           user_id,
                           name
                         FROM public.category
                         WHERE user_id = @user_id
                         """;

      await using var connection = new NpgsqlConnection(ConnectionString);

      var p = new DynamicParameters();
      AddUserIdParameter(p, userId);

      return await connection.QueryAsync<CategoryEntity>(sql, p);
    }

    public async Task<bool> Exists(int userId, int categoryId)
    {
      const string sql = """
                         SELECT EXISTS (
                         SELECT 1
                         FROM public.category
                         WHERE user_id = @user_id AND category_id = @category_id
                         );
                         """;

      await using var connection = new NpgsqlConnection(ConnectionString);

      var p = GetPrimaryKeyParameter(categoryId);
      AddUserIdParameter(p, userId);
      
      var exists = (bool)(await connection.ExecuteScalarAsync(sql, p));

      return exists;
    }

    public async Task<bool> Exists(int userId, string name)
    {
      const string sql = """
                         SELECT EXISTS (
                         SELECT 1
                         FROM public.category
                         WHERE user_id = @user_id AND name = @name
                         );
                         """;

      await using var connection = new NpgsqlConnection(ConnectionString);

      var p = new DynamicParameters();
      AddUserIdParameter(p, userId);

      p.Add(
        "@name",
        dbType: DbType.String,
        value: name,
        size: 20);

      var exists = (bool)(await connection.ExecuteScalarAsync(sql, p));

      return exists;
    }

    public async Task<int> Insert(CategoryEntity entity)
    {
      const string sql = @"INSERT INTO public.category (
                user_id,
                name
						) VALUES (
                @user_id,
                @name
                )	RETURNING category_id AS PK;";

      await using var connection = new NpgsqlConnection(ConnectionString);

      var p = new DynamicParameters();
      AddUserIdParameter(p, entity.UserId);

      p.Add(
        "@name",
        dbType: DbType.String,
        value: entity.Name,
        size: 20);

      return await connection.ExecuteScalarAsync<int>(sql, p);
    }

    public async Task Update(CategoryEntity entity)
    {
      const string sql = """
                            UPDATE public.category SET 
                              name = @name,
                              modified_on = now()
                         		WHERE user_id = @user_id 
                         		  AND category_id = @category_id
                         """;

      await using var connection = new NpgsqlConnection(ConnectionString);

      var p = GetPrimaryKeyParameter(entity.CategoryId);
      AddUserIdParameter(p, entity.UserId);

      p.Add(
        "@name",
        dbType: DbType.String,
        value: entity.Name,
        size: 20);

      await connection.ExecuteAsync(sql, p);
    }

    public async Task Delete(int userId, int categoryId)
    {
      const string sql = "DELETE FROM public.category WHERE category_id = @category_id";

      await using var connection = new NpgsqlConnection(ConnectionString);

      var p = GetPrimaryKeyParameter(categoryId);
      AddUserIdParameter(p, userId);

      await connection.ExecuteAsync(sql, p);
    }

    private DynamicParameters GetPrimaryKeyParameter(int categoryId)
    {
      var p = new DynamicParameters();
      p.Add("@category_id", dbType: DbType.Int32, value: categoryId);

      return p;
    }
  }
}
