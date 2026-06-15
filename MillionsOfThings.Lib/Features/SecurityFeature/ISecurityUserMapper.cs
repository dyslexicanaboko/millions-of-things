using MillionsOfThings.Lib.Features.UserFeature.Models;

namespace MillionsOfThings.Lib.Features.SecurityFeature;

public interface ISecurityUserMapper
{
  SecurityUserCreateEntity? ToEntity(
    UserV1CreateModel? target,
    bool isAllowed,
    string password,
    Guid securityRoleId);

  SecurityUserCreateRecord ToRecord(SecurityUserCreateEntity target);
}
