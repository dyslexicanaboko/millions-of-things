using MillionsOfThings.Lib.Features.SecurityFeature.Authenticated;
using MillionsOfThings.Lib.Utility;

namespace MillionsOfThings.Lib.Features.UserFeature
{
  public interface IUserManager
  {
    Task<List<UserEntity>> GetAll(ClaimsUserModel currentUser);

    Task<UserEntity?> Get(int id);

    Task PartialEdit(int userId, List<UpdateInstruction> instructions);

    Task Remove(int id);
  }
}
