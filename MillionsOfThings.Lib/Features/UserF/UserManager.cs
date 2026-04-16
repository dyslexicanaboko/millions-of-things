namespace MillionsOfThings.Lib.Features.UserF
{
  public class UserManager : IUserManager
  {
    private readonly IUserRepository _repoUser;

    public UserManager(
      IUserRepository repoUser)
      => _repoUser = repoUser;

    public async Task<UserEntity?> GetUser(int id)
    {
      //Password is purposely not returned
      var dbEntity = await _repoUser.Select(id);

      return dbEntity;
    }

    public async Task<List<UserEntity>> GetAllUsers()
    {
      //Password is purposely not returned
      var lst = (await _repoUser.SelectAll()).ToList();

      return lst;
    }

    public async Task<UserEntity> Add(UserEntity? user)
    {
      Validations.IsNotNull(user, nameof(user));

      user.UserId = await _repoUser.Insert(user);

      return user;
    }
  }
}
