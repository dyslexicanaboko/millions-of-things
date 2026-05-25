using Dapper;
using System.Data;

namespace MillionsOfThings.Lib.Features.Security;

public class SecurityUserRepository
  : BaseRepository, ISecurityUserRepository
{
  public SecurityUserRepository(IAppConfiguration configuration)
    : base(configuration)
  {
  }

  /* 2026-05-25 Maybe the rest of the CUD will be needed in the future.
   * I am not sure yet.
   */

  //NOTE: Search by exact username, this is where the CITEXT type may be required later
  public async Task<SecurityUserRecord?> Read(string username)
  {
    //This is the only situation where password will be returned
    //so it can be used with authentication.
    const string sql = """
                       SELECT
                         u.user_id,
                         u.is_allowed,
                         u.username,
                         u.password,
                         u.created_on,
                         sr.security_role_id,
                         sr.role
                       FROM public.user u
                        inner join public.security_role sr 
                          on u.security_role_id = sr.security_role_id
                       WHERE u.username = @username
                       """;

    await using var connection = await GetConnection();

    var p = new DynamicParameters();
    p.Add("username", username, DbType.String, size:20);

    return await connection.QuerySingleOrDefaultAsync<SecurityUserRecord>(sql, p);
  }

  public async Task<SecurityUserRecord?> Read(int userId)
  {
    //This is the only situation where password will be returned
    //so it can be used with authentication.
    const string sql = """
                       SELECT
                         u.user_id,
                         u.is_allowed,
                         u.username,
                         '' as password,
                         u.created_on,
                         sr.security_role_id,
                         sr.role
                       FROM public.user u
                        inner join public.security_role sr 
                          on u.security_role_id = sr.security_role_id
                       WHERE u.user_id = @user_id
                       """;

    await using var connection = await GetConnection();

    var p = new DynamicParameters();
    p.Add("user_id", userId, DbType.Int32);

    return await connection.QuerySingleOrDefaultAsync<SecurityUserRecord>(sql, p);
  }
}