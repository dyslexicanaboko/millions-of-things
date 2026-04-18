using MillionsOfThings.Lib.Utility;

namespace MillionsOfThings.Lib.Features.UserF
{
  public interface IUserManager
  {
    Task<UserEntity> Add(UserEntity? user);

    Task<List<UserEntity>> GetAll();

    Task<UserEntity?> Get(int id);

    Task PartialEdit(int userId, List<UpdateInstruction> instructions);

    Task Remove(int id);
  }
}
