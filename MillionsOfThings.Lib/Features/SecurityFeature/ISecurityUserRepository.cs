namespace MillionsOfThings.Lib.Features.SecurityFeature;

public interface ISecurityUserRepository : IRepository
{
  Task<SecurityUserRecord?> Read(string username);

  Task<SecurityUserRecord?> Read(int userId);

  Task<List<SecurityPermissionRecord>> ReadPermissions(Guid securityRoleId);

  Task<int> Create(SecurityUserCreateRecord record);

  Task<bool> DoesUsernameExist(string username);

  Task<bool> DoesEmailAddressExist(string emailAddress);

  Task<Guid?> ReadSecurityRole(string role);
}