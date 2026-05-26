using Dapper;
using System.Data;

namespace MillionsOfThings.Lib.Features.CategoryFeature
{
  public class CategoryRepository(IAppConfiguration configuration) 
    : BaseRepository(configuration), ICategoryRepository
  {
    public async Task<CategoryRecord?> Read(int userId, int categoryId)
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

      await using var connection = await GetConnection();

      var p = GetPrimaryKeyParameter(categoryId);
      AddUserIdParameter(p, userId);

      return (await connection.QueryAsync<CategoryRecord>(sql, p)).SingleOrDefault();
    }

    public async Task<List<CategoryRecord>> ReadAll()
    {
      const string sql = """
                         SELECT
                           category_id,
                           user_id,
                           name
                         FROM public.category
                         """;

      await using var connection = await GetConnection();

      return (await connection.QueryAsync<CategoryRecord>(sql)).ToList();
    }

    public async Task<List<CategoryRecord>> ReadAll(int userId)
    {
      const string sql = """
                         SELECT
                           category_id,
                           user_id,
                           name
                         FROM public.category
                         WHERE user_id = @user_id
                         """;

      await using var connection = await GetConnection();

      var p = new DynamicParameters();
      AddUserIdParameter(p, userId);

      return (await connection.QueryAsync<CategoryRecord>(sql, p)).ToList();
    }

    public async Task<int> UsageCount(int userId, int categoryId)
    {
      const string sql = """
                         SELECT COUNT(task_id)
                         FROM public.task
                         WHERE user_id = @user_id AND category_id = @category_id
                         """;

      await using var connection = await GetConnection();

      var p = GetPrimaryKeyParameter(categoryId);
      AddUserIdParameter(p, userId);

      var result = await connection.ExecuteScalarAsync(sql, p);
      var count = result is null ? 0 : Convert.ToInt32(result);

      return count;
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

      await using var connection = await GetConnection();

      var p = GetPrimaryKeyParameter(categoryId);
      AddUserIdParameter(p, userId);
      
      var exists = await connection.ExecuteScalarAsync<bool>(sql, p);

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

      await using var connection = await GetConnection();

      var p = new DynamicParameters();
      AddUserIdParameter(p, userId);

      p.Add(
        "@name",
        dbType: DbType.String,
        value: name,
        size: 20);

      var exists = await connection.ExecuteScalarAsync<bool>(sql, p);

      return exists;
    }

    public async Task<int> Create(CategoryRecord entity)
    {
      const string sql = """
                         INSERT INTO public.category (
                                         user_id,
                                         name
                                    ) VALUES (
                                         @user_id,
                                         @name
                                         )	RETURNING category_id AS PK;
                         """;

      await using var connection = await GetConnection();

      var p = new DynamicParameters();
      AddUserIdParameter(p, entity.UserId);

      p.Add(
        "@name",
        dbType: DbType.String,
        value: entity.Name,
        size: 20);

      return await connection.ExecuteScalarAsync<int>(sql, p);
    }

    public async Task Update(CategoryRecord entity)
    {
      const string sql = """
                            UPDATE public.category SET 
                              name = @name,
                              modified_on = now()
                            WHERE user_id = @user_id 
                              AND category_id = @category_id
                         """;

      await using var connection = await GetConnection();

      var p = GetPrimaryKeyParameter(entity.CategoryId);
      AddUserIdParameter(p, entity.UserId);

      p.Add(
        "@name",
        dbType: DbType.String,
        value: entity.Name,
        size: 20);

      await connection.ExecuteAsync(sql, p);
    }

    //Until this is needed again later, I am going to keep it commented out.
    //I can't remember right now, but I think I needed this separately,
    //I just don't remember why.
    //public async Task<int> DetachFromTasks(int userId, int categoryId)
    //{
    //  const string sql = """
    //                     UPDATE public.task SET
    //                       category_id = NULL
    //                      ,modified_on = now()
    //                     WHERE user_id = @user_id AND category_id = @category_id
    //                     """;

    //  await using var connection = await GetConnection();

    //  var p = GetPrimaryKeyParameter(categoryId);
    //  AddUserIdParameter(p, userId);

    //  var result = await connection.ExecuteScalarAsync(sql, p);
    //  var count = result is null ? 0 : Convert.ToInt32(result);

    //  return count;
    //}

    /// <summary>
    /// Deletes the category with the specified identifier for the given user.
    /// </summary>
    /// <param name="userId">The identifier of the user who owns the category to be deleted.</param>
    /// <param name="categoryId">The identifier of the category to delete.</param>
    /// <returns>The number of categories deleted. Returns 0 if no matching category was found.</returns>
    public async Task<(bool, int)> Delete(int userId, int categoryId)
    {
      var p = GetPrimaryKeyParameter(categoryId);
      AddUserIdParameter(p, userId);

      await using var connection = await GetConnection();

      var tran = await connection.BeginTransactionAsync();

      //Detach the category from any tasks that are using it
      const string sqlDetach = """
                               UPDATE public.task SET
                                 category_id = NULL
                                ,modified_on = now()
                               WHERE user_id = @user_id AND category_id = @category_id
                               """;

      var resultDetach = await connection.ExecuteScalarAsync(sqlDetach, p, tran);
      var countDetached = resultDetach is null ? 0 : Convert.ToInt32(resultDetach);

      //Now delete the category itself
      const string sqlDelete = "DELETE FROM public.category WHERE user_id = @user_id AND category_id = @category_id";
      
      var result = await connection.ExecuteScalarAsync(sqlDelete, p, tran);
      var isSuccess = (result is null ? 0 : Convert.ToInt32(result)) > 0;

      if (isSuccess)
        await tran.CommitAsync();
      else
        await tran.RollbackAsync();

      return (isSuccess, countDetached);
    }

    private static DynamicParameters GetPrimaryKeyParameter(int categoryId)
    {
      var p = new DynamicParameters();
      p.Add("@category_id", dbType: DbType.Int32, value: categoryId);

      return p;
    }
  }
}
