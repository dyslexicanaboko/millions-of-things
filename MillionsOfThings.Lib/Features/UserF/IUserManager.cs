namespace MillionsOfThings.Lib.Features.UserF
{
  public interface IUserManager
  {
    Task<UserEntity> Add(UserEntity? user);

    Task<List<UserEntity>> GetAll();

    Task<UserEntity?> Get(int id);
  }
}
