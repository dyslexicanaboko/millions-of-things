namespace MillionsOfThings.Lib.Features.Security
{
  public interface IUserManager
  {
    Task<UserEntity> Add(UserEntity? user);

    Task<List<UserEntity>> GetAllUsers();

    Task<UserEntity?> GetUser(int id);
  }
}
