namespace MillionsOfThings.Lib.Features.SecurityFeature;

public interface ISecurityUserRepository : IRepository
{
  Task<SecurityUserRecord?> Read(string username, CancellationToken cancellationToken);

  Task<SecurityUserRecord?> Read(int userId, CancellationToken cancellationToken);

  Task<List<SecurityPermissionRecord>> ReadPermissions(Guid securityRoleId, CancellationToken cancellationToken);

  Task<int> Create(SecurityUserCreateRecord record, CancellationToken cancellationToken);

  Task<bool> DoesUsernameExist(string username, CancellationToken cancellationToken);

  Task<bool> DoesEmailAddressExist(string emailAddress, CancellationToken cancellationToken);

  Task<Guid?> ReadSecurityRole(string role, CancellationToken cancellationToken);
}