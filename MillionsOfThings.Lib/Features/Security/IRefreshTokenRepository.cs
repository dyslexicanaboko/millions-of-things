namespace MillionsOfThings.Lib.Features.Security
{
  public interface IRefreshTokenRepository
  {
    Task Delete(int userId, string token);
    Task DeleteExpired(int userId);
    Task<Guid> Insert(RefreshTokenEntity entity);
    Task<RefreshTokenEntity?> Select(Guid refreshTokenId);
    Task<RefreshTokenEntity?> Select(string token);
    Task<IEnumerable<RefreshTokenEntity>> SelectAll();
    Task Update(RefreshTokenEntity entity);
  }
}