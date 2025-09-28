using MillionsOfThings.Lib.DataAccess;
using MillionsOfThings.Lib.DataAccess.Security;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Validation;

namespace MillionsOfThings.Lib.Services.Security
{
  public class UserService : IUserService
  {
    private readonly IUserRepository _repoUser;

    public UserService(
      IUserRepository repoUser)
      => _repoUser = repoUser;

    public async Task<UserEntity?> GetUser(int id)
    {
      //Password is purposely not returned
      var dbEntity = await _repoUser.Using(x => x.Select(id));

      return dbEntity;
    }

    public async Task<IList<UserEntity>> GetAllUsers()
    {
      //Password is purposely not returned
      var lst = (await _repoUser.Using(x => x.SelectAll())).ToList();

      return lst;
    }

    public async Task<UserEntity> Add(UserEntity? user)
    {
      Validations.IsNotNull(user, nameof(user));

      user.UserId = await _repoUser.Using(x => x.Insert(user));

      return user;
    }
  }
}
