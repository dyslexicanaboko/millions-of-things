namespace MillionsOfThings.Lib.Features.SecurityFeature;

public interface ISecurityUserRepository : IRepository
{
  Task<SecurityUserRecord?> Read(string username);

  Task<SecurityUserRecord?> Read(int userId);

  Task<List<SecurityPermissionRecord>> ReadPermissions(Guid securityRoleId);

  Task<int> Create(SecurityUserCreateRecord record);
}