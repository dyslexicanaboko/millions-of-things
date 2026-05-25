namespace MillionsOfThings.Lib.Features.Security;

public interface ISecurityUserRepository : IRepository
{
  Task<SecurityUserRecord?> Read(string username);

  Task<SecurityUserRecord?> Read(int userId);
}