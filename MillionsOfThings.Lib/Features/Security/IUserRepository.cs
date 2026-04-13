using MillionsOfThings.Lib.Features;

namespace MillionsOfThings.Lib.Features.Security
{
  public interface IUserRepository : IRepository
  {
    Task Delete(int userId);
    Task<int> Insert(UserEntity entity);
    Task<UserEntity?> Select(int userId);
    Task<UserEntity?> Select(string username);
    Task<IEnumerable<UserEntity>> SelectAll();
    Task Update(UserEntity entity);
  }
}