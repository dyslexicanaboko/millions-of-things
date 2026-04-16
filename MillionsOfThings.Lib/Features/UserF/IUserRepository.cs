namespace MillionsOfThings.Lib.Features.UserF
{
  public interface IUserRepository : IRepository
  {
    Task Delete(int userId);
    Task<int> Insert(UserRecord entity);
    Task<UserRecord?> Select(int userId);
    Task<UserRecord?> Select(string username);
    Task<List<UserRecord>> SelectAll();
    Task Update(UserRecord entity);
  }
}