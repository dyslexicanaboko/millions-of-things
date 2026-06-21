using Dapper;
using System.Data;

namespace MillionsOfThings.Lib.Features.SecurityFeature;

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

  public async Task<SecurityUserRecord?> Read(string username, CancellationToken cancellationToken)
  {
    //This is the only situation where password will be returned
    //so it can be used with authentication.
    const string sql = """
                       SELECT
                         u.user_id,
                         u.is_allowed,
                         u.firstname,
                         u.lastname,
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

    var p = new DynamicParameters();
    p.Add("username", username, DbType.String, size: 20);

    await using var connection = await GetConnection(cancellationToken);
    
    return await connection.QuerySingleOrDefaultAsync<SecurityUserRecord>(sql, p);
  }

  public async Task<SecurityUserRecord?> Read(int userId, CancellationToken cancellationToken)
  {
    //Password is not needed here, so blanking it out.
    const string sql = """
                       SELECT
                         u.user_id,
                         u.is_allowed,
                         u.firstname,
                         u.lastname,
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

    var p = new DynamicParameters();
    p.Add("user_id", userId, DbType.Int32);

    await using var connection = await GetConnection(cancellationToken);
    
    return await connection.QuerySingleOrDefaultAsync<SecurityUserRecord>(sql, p);
  }

  public async Task<List<SecurityPermissionRecord>> ReadPermissions(Guid securityRoleId, CancellationToken cancellationToken)
  {
    const string sql = """
                       select
                        sp.permission
                       from public.security_role_permission_link lnk
                        inner join security_permission sp 
                            on lnk.security_permission_id = sp.security_permission_id
                       where lnk.security_role_id = @security_role_id
                       """;

    var p = new DynamicParameters();
    p.Add("security_role_id", securityRoleId, DbType.Guid);

    await using var connection = await GetConnection(cancellationToken);

    return (await connection.QueryAsync<SecurityPermissionRecord>(sql, p)).ToList();
  }

  public async Task<int> Create(SecurityUserCreateRecord record, CancellationToken cancellationToken)
  {
    const string sql = """
                       INSERT INTO public.user (
                         is_allowed,
                         username,
                         password,
                         firstname,
                         lastname,
                         emailaddress,
                         security_role_id
                       ) VALUES (
                         @is_allowed,
                         @username,
                         @password,
                         @firstname,
                         @lastname,
                         @emailaddress,
                         @security_role_id
                       ) RETURNING user_id AS PK;
                       """;

    await using var connection = await GetConnection(cancellationToken);

    var p = new DynamicParameters();
    p.Add(name: "@is_allowed", dbType: DbType.Boolean, value: record.IsAllowed);
    p.Add(name: "@username", dbType: DbType.String, value: record.Username, size: 20);
    p.Add(name: "@password", dbType: DbType.String, value: record.Password, size: 100);
    p.Add(name: "@firstname", dbType: DbType.String, value: record.FirstName, size: 50);
    p.Add(name: "@lastname", dbType: DbType.String, value: record.LastName, size: 50);
    p.Add(name: "@emailaddress", dbType: DbType.String, value: record.EmailAddress, size: 100);
    p.Add(name: "@security_role_id", dbType: DbType.Guid, value: record.SecurityRoleId);

    return await connection.ExecuteScalarAsync<int>(sql, p);
  }

  public async Task<bool> DoesUsernameExist(string username, CancellationToken cancellationToken)
  {
    const string sql = """
                       SELECT EXISTS (
                         SELECT 1
                         FROM public.user
                         WHERE username = @username
                       )
                       """;

    var p = new DynamicParameters();
    p.Add("username", username, DbType.String, size: 20);

    await using var connection = await GetConnection(cancellationToken);

    return await connection.QuerySingleAsync<bool>(sql, p);
  }

  public async Task<bool> DoesEmailAddressExist(string emailAddress, CancellationToken cancellationToken)
  {
    const string sql = """
                       SELECT EXISTS (
                         SELECT 1
                         FROM public.user
                         WHERE emailaddress = @emailaddress
                       )
                       """;

    var p = new DynamicParameters();
    p.Add("emailaddress", emailAddress, DbType.String, size: 100);

    await using var connection = await GetConnection(cancellationToken);

    return await connection.QuerySingleAsync<bool>(sql, p);
  }

  public async Task<Guid?> ReadSecurityRole(string role, CancellationToken cancellationToken)
  {
    const string sql = """
                       SELECT security_role_id
                       FROM public.security_role
                       WHERE role = @role::citext
                       """;

    var p = new DynamicParameters();
    p.Add("role", role, DbType.String);

    await using var connection = await GetConnection(cancellationToken);

    //Cancellation tokens are not supported for most Dapper methods yet
    // https://github.com/DapperLib/Dapper/issues/1181
    return await connection.QuerySingleOrDefaultAsync<Guid?>(sql, p);
  }
}