using Dapper;
using System.Data;

namespace MillionsOfThings.Lib.Features.UserF;

public class UserRepository
  : BaseRepository, IUserRepository
{
  public UserRepository(IAppConfiguration configuration)
    : base(configuration)
  {
  }

  public async Task<UserRecord?> Select(int userId)
  {
    //Password is purposely not included here
    const string sql = """
                       SELECT
                         user_id,
                         is_allowed,
                         username,
                         created_on
                       FROM public.user
                       WHERE user_id = @user_id
                       """;

    await using var connection = await GetConnection();

    return await connection.QuerySingleOrDefaultAsync<UserRecord>(sql, GetPrimaryKeyParameter(userId));
  }

  //NOTE: Security related call
  // Search by exact username, this is where the CITEXT type may be required later
  public async Task<UserRecord?> Select(string username)
  {
    //This is the only situation where password will be returned
    //so it can be used with authentication.
    const string sql = """
                       SELECT
                         user_id,
                         is_allowed,
                         username,
                         password,
                         created_on
                       FROM public.user
                       WHERE username = @username
                       """;

    await using var connection = await GetConnection();

    return await connection.QuerySingleOrDefaultAsync<UserRecord>(sql, new { username });
  }

  public async Task<List<UserRecord>> SelectAll()
  {
    const string sql = """
                       SELECT
                         user_id,
                         is_allowed,
                         username,
                         created_on
                       FROM public.user
                       """;

    await using var connection = await GetConnection();

    return (await connection.QueryAsync<UserRecord>(sql)).AsList();
  }

  public async Task<int> Insert(UserRecord entity)
  {
    const string sql = """
                       INSERT INTO public.user (
                         is_allowed,
                         username,
                         password
                       ) VALUES (
                         @is_allowed,
                         @username,
                         @password
                       ) RETURNING user_id AS PK;
                       """;

    await using var connection = await GetConnection();

    var p = new DynamicParameters();
    p.Add(name: "@is_allowed", dbType: DbType.Boolean, value: entity.IsAllowed);
    p.Add(name: "@username", dbType: DbType.String, value: entity.Username, size: 20);
    p.Add(name: "@password", dbType: DbType.String, value: entity.Password, size: 100);

    return await connection.ExecuteScalarAsync<int>(sql, p);
  }

  //Not sure if what is being updated here is correct yet
  public async Task Update(UserRecord entity)
  {
    const string sql = """
                       UPDATE public.user SET
                         is_allowed = @is_allowed,
                         username = @username,
                         password = @password,
                         modified_on = now()
                       WHERE user_id = @user_id
                       """;

    await using var connection = await GetConnection();

    var p = new DynamicParameters();
    p.Add(name: "@user_id", dbType: DbType.Int32, value: entity.UserId);
    p.Add(name: "@is_allowed", dbType: DbType.Boolean, value: entity.IsAllowed);
    p.Add(name: "@username", dbType: DbType.String, value: entity.Username, size: 20);
    p.Add(name: "@password", dbType: DbType.String, value: entity.Password, size: 100);

    await connection.ExecuteAsync(sql, p);
  }

  public async Task Delete(int userId)
  {
    const string sql = "DELETE FROM public.user WHERE user_id = @user_id";

    await using var connection = await GetConnection();

    await connection.ExecuteAsync(sql, GetPrimaryKeyParameter(userId));
  }

  private static DynamicParameters GetPrimaryKeyParameter(int userId)
  {
    var p = new DynamicParameters();
    p.Add(name: "@user_id", dbType: DbType.Int32, value: userId);

    return p;
  }
}