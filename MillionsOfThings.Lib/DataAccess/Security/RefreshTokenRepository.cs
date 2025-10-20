using Dapper;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Services;
using System.Data;

namespace MillionsOfThings.Lib.DataAccess.Security
{
  public class RefreshTokenRepository
    : BaseRepository, IRefreshTokenRepository
  {
    public RefreshTokenRepository(IAppConfiguration configuration)
        : base(configuration)
    {
    }

    public async Task<RefreshTokenEntity?> Select(Guid refreshTokenId)
    {
      const string sql = @"
			SELECT
	                refresh_token_id,
                user_id,
                token,
                created_by_ip,
                created_on
			FROM public.refresh_token
			WHERE refresh_token_id = @refresh_token_id";

      var connection = await GetConnection();

      var lst = (await connection.QueryAsync<RefreshTokenEntity>(sql, GetPrimaryKeyParameter(refreshTokenId))).ToList();

      return lst.SingleOrDefault();
    }

    public async Task<RefreshTokenEntity?> Select(string token)
    {
      const string sql = @"
			SELECT
	              refresh_token_id,
                user_id,
                token,
                created_by_ip,
                created_on
			FROM public.refresh_token
			WHERE token = @token";

      var connection = await GetConnection();

      var lst = (await connection.QueryAsync<RefreshTokenEntity>(sql, new { token } )).ToList();

      return lst.SingleOrDefault();
    }

    public async Task<IEnumerable<RefreshTokenEntity>> SelectAll()
    {
      const string sql = @"
			SELECT
	                refresh_token_id,
                user_id,
                token,
                created_by_ip,
                created_on
			FROM public.refresh_token";

      var connection = await GetConnection();

      return (await connection.QueryAsync<RefreshTokenEntity>(sql)).ToList();
    }

    public async Task<Guid> Insert(RefreshTokenEntity entity)
    {
      const string sql = @"INSERT INTO public.refresh_token (
                refresh_token_id,
                user_id,
                token,
                created_by_ip
						) VALUES (
                @refresh_token_id,
                @user_id,
                @token,
                @created_by_ip)	";

      var connection = await GetConnection();

      var p = new DynamicParameters();
      p.Add(name: "@refresh_token_id", dbType: DbType.Guid, value: entity.RefreshTokenId);
      p.Add(name: "@user_id", dbType: DbType.Int32, value: entity.UserId);
      p.Add(name: "@token", dbType: DbType.String, value: entity.Token, size: 255);
      p.Add(name: "@created_by_ip", dbType: DbType.String, value: entity.CreatedByIp, size: 39);

      await connection.ExecuteAsync(sql, p);

      return entity.RefreshTokenId;
    }

    public async Task Update(RefreshTokenEntity entity)
    {
      const string sql = @"UPDATE public.refresh_token SET 
                token = @token,
                created_by_ip = @created_by_ip,
                modified_on = now()
						WHERE refresh_token_id = @refresh_token_id";

      var connection = await GetConnection();

      var p = new DynamicParameters();
      p.Add(name: "@refresh_token_id", dbType: DbType.Guid, value: entity.RefreshTokenId);
      p.Add(name: "@token", dbType: DbType.String, value: entity.Token, size: 255);
      p.Add(name: "@created_by_ip", dbType: DbType.String, value: entity.CreatedByIp, size: 39);

      await connection.ExecuteAsync(sql, p);
    }

    public async Task Delete(int userId, string token)
    {
      const string sql = """
        DELETE FROM public.refresh_token 
        WHERE refresh_token_id = @refresh_token_id and user_id = @user_id
        """;

      var connection = await GetConnection();

      var p = new DynamicParameters();
      p.Add(name: "@refresh_token_id", dbType: DbType.String, value: token, size: 255);
      p.Add(name: "@user_id", dbType: DbType.Int32, value: userId);

      await connection.ExecuteAsync(sql, p);
    }

    public async Task DeleteExpired(int userId)
    {
      const string sql = """
        DELETE FROM public.refresh_token 
        WHERE user_id = @user_id
        """;

      var connection = await GetConnection();

      var p = new DynamicParameters();
      p.Add(name: "@user_id", dbType: DbType.Int32, value: userId);

      await connection.ExecuteAsync(sql, p);
    }

    private static DynamicParameters GetPrimaryKeyParameter(Guid refreshTokenId)
    {
      var p = new DynamicParameters();
      p.Add(name: "@refresh_token_id", dbType: DbType.Guid, value: refreshTokenId);

      return p;
    }
  }
}
