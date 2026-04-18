using MillionsOfThings.Lib.Utility;

namespace MillionsOfThings.Lib.Features.UserF
{
  public interface IUserRepository : IRepository
  {
    Task Delete(int userId);
    Task<int> Create(UserRecord entity);
    Task<UserRecord?> Read(int userId);
    Task<UserRecord?> Read(string username);
    Task<List<UserRecord>> ReadAll();

    Task UpdatePartial(int userId, List<UpdateInstruction> updateInstructions);
  }
}