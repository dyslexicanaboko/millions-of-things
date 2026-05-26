using Dapper;
using MillionsOfThings.Lib.Utility;
using System.Data;

namespace MillionsOfThings.Lib.Features.UserFeature;

public class UserRepository
  : BaseRepository, IUserRepository
{
  public UserRepository(IAppConfiguration configuration)
    : base(configuration)
  {
  }

  //NOTE: Password is purposely not exposed in this repository
  
  public async Task<UserRecord?> Read(int userId)
  {
    const string sql = """
                       SELECT
                         user_id,
                         is_allowed,
                         username,
                         firstname,
                         lastname,
                         emailaddress,
                         created_on
                       FROM public.user
                       WHERE user_id = @user_id
                       """;

    await using var connection = await GetConnection();

    return await connection.QuerySingleOrDefaultAsync<UserRecord>(sql, GetPrimaryKeyParameter(userId));
  }

  //TODO: Needs to be pageable. Only accessible by administrators.
  public async Task<List<UserRecord>> ReadAll()
  {
    const string sql = """
                       SELECT
                         user_id,
                         is_allowed,
                         username,
                         firstname,
                         lastname,
                         emailaddress,
                         created_on
                       FROM public.user
                       """;

    await using var connection = await GetConnection();

    return (await connection.QueryAsync<UserRecord>(sql)).AsList();
  }

  //TODO: This might move to the security version, not sure yet.
  public async Task<int> Create(UserRecord entity)
  {
    const string sql = """
                       INSERT INTO public.user (
                         is_allowed,
                         username,
                       -- password,
                         firstname,
                         lastname,
                         emailaddress
                       ) VALUES (
                         @is_allowed,
                         @username,
                       -- @password,
                         @firstname,
                         @lastname,
                         @emailaddress,
                       ) RETURNING user_id AS PK;
                       """;

    await using var connection = await GetConnection();

    var p = new DynamicParameters();
    p.Add(name: "@is_allowed", dbType: DbType.Boolean, value: entity.IsAllowed);
    p.Add(name: "@username", dbType: DbType.String, value: entity.Username, size: 20);
    //p.Add(name: "@password", dbType: DbType.String, value: entity.Password, size: 100);
    p.Add(name: "@firstname", dbType: DbType.String, value: entity.FirstName, size: 50);
    p.Add(name: "@lastname", dbType: DbType.String, value: entity.LastName, size: 50);
    p.Add(name: "@emailaddress", dbType: DbType.String, value: entity.EmailAddress, size: 100);

    return await connection.ExecuteScalarAsync<int>(sql, p);
  }

  private static readonly List<ColumnSchema> UpdateableColumns = [
    new ("IsAllowed", "is_allowed", DbType.Boolean),
    //new ("Password", "password", DbType.String, 100),
    new ("FirstName", "firstname", DbType.String, 50),
    new ("LastName", "lastname", DbType.String, 50),
    new ("EmailAddress", "emailaddress", DbType.String, 100),
  ];

  public async Task UpdatePartial(int userId, List<UpdateInstruction> updateInstructions)
  {
    const string sql = """
                       UPDATE public.user SET
                         {0}
                         modified_on = now()
                       WHERE user_id = @user_id
                       """;

    var p = GetPrimaryKeyParameter(userId);

    await UpdatePartial(
      sql,
      UpdateableColumns,
      p,
      updateInstructions);
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