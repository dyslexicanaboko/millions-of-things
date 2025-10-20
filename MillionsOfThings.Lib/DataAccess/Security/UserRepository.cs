using Dapper;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Services;
using Npgsql;
using System.Data;

namespace MillionsOfThings.Lib.DataAccess.Security
{
  public class UserRepository
    : BaseRepository, IUserRepository
  {
    public UserRepository(IAppConfiguration configuration)
        : base(configuration)
    {
    }

    public async Task<UserEntity?> Select(int userId)
    {
      //Password is purposely not included here
      const string sql = @"
			SELECT
	                user_id,
                is_allowed,
                username,
                created_on
			FROM public.user
			WHERE user_id = @user_id";

      var connection = await GetConnection();

      var lst = (await connection.QueryAsync<UserEntity>(sql, GetPrimaryKeyParameter(userId))).ToList();

      return lst.SingleOrDefault();
    }

    public async Task<UserEntity?> Select(string username)
    {
      //This is the only situation where password will be returned
      //so it can be used with authentication.
      const string sql = @"
			SELECT
	                user_id,
                is_allowed,
                username,
                password,
                created_on
			FROM public.user
			WHERE username = @username";

      var connection = await GetConnection();

      var lst = (await connection.QueryAsync<UserEntity>(sql, new { username } )).ToList();

      return lst.SingleOrDefault();
    }

    public async Task<IEnumerable<UserEntity>> SelectAll()
    {
      const string sql = @"
			SELECT
	                user_id,
                is_allowed,
                username,
                created_on
			FROM public.user";

      var connection = await GetConnection();

      return (await connection.QueryAsync<UserEntity>(sql)).ToList();
    }

    public async Task<int> Insert(UserEntity entity)
    {
      const string sql = @"INSERT INTO public.user (
                is_allowed,
                username,
                password
						) VALUES (
                @is_allowed,
                @username,
                @password)	RETURNING user_id AS PK;";

      var connection = await GetConnection();

      var p = new DynamicParameters();
      p.Add(name: "@is_allowed", dbType: DbType.Boolean, value: entity.IsAllowed);
      p.Add(name: "@username", dbType: DbType.String, value: entity.Username, size: 20);
      p.Add(name: "@password", dbType: DbType.String, value: entity.Password, size: 100);

      return await connection.ExecuteScalarAsync<int>(sql, p);
    }

    //Not sure if what is being updated here is correct yet
    public async Task Update(UserEntity entity)
    {
      const string sql = @"UPDATE public.user SET 
	                is_allowed = @is_allowed,
                username = @username,
                password = @password,
                modified_on = now()
						WHERE user_id = @user_id";

      var connection = await GetConnection();

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

      var connection = await GetConnection();

      await connection.ExecuteAsync(sql, GetPrimaryKeyParameter(userId));
    }

    private DynamicParameters GetPrimaryKeyParameter(int userId)
    {
      var p = new DynamicParameters();
      p.Add(name: "@user_id", dbType: DbType.Int32, value: userId);

      return p;
    }
  }
}

