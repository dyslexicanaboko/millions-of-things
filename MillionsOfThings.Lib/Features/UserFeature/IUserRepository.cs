using MillionsOfThings.Lib.Utility;

namespace MillionsOfThings.Lib.Features.UserFeature
{
  public interface IUserRepository : IRepository
  {
    Task Delete(int userId);
    
    Task<UserRecord?> Read(int userId);
    
    Task<List<UserRecord>> ReadAll();

    Task UpdatePartial(int userId, List<UpdateInstruction> updateInstructions);
  }
}