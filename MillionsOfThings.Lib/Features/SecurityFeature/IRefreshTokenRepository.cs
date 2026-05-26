namespace MillionsOfThings.Lib.Features.SecurityFeature
{
  public interface IRefreshTokenRepository
  {
    Task Delete(int userId, string token);
    Task DeleteExpired(int userId);
    Task<Guid> Create(RefreshTokenRecord record);
    Task<RefreshTokenRecord?> Read(Guid refreshTokenId);
    Task<RefreshTokenRecord?> Read(string token);
    Task<List<RefreshTokenRecord>> ReadAll();
    Task Update(RefreshTokenRecord record);
  }
}