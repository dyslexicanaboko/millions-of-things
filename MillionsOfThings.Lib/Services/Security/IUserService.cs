using MillionsOfThings.Lib.Entities;

namespace MillionsOfThings.Lib.Services.Security
{
  public interface IUserService
  {
    Task<UserEntity> Add(UserEntity? user);

    Task<List<UserEntity>> GetAllUsers();

    Task<UserEntity?> GetUser(int id);
  }
}
