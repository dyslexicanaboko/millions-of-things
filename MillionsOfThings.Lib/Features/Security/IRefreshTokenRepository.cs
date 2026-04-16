namespace MillionsOfThings.Lib.Features.Security
{
  public interface IRefreshTokenRepository
  {
    Task Delete(int userId, string token);
    Task DeleteExpired(int userId);
    Task<Guid> Insert(RefreshTokenRecord record);
    Task<RefreshTokenRecord?> Select(Guid refreshTokenId);
    Task<RefreshTokenRecord?> Select(string token);
    Task<List<RefreshTokenRecord>> SelectAll();
    Task Update(RefreshTokenRecord record);
  }
}