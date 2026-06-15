using MillionsOfThings.Lib.Features.UserFeature.Models;

namespace MillionsOfThings.Lib.Features.SecurityFeature;

public class SecurityUserMapper
  : BaseMapper, ISecurityUserMapper
{
  public SecurityUserCreateEntity? ToEntity(
    UserV1CreateModel? target,
    bool isAllowed,
    string password,
    Guid securityRoleId)
    => target == null ? 
      null : 
      new SecurityUserCreateEntity(target, isAllowed, password, securityRoleId);

  public SecurityUserCreateRecord ToRecord(SecurityUserCreateEntity target)
  {
    return new SecurityUserCreateRecord
    {
      Username = target.Username,
      Password = target.Password,
      FirstName = target.FirstName,
      LastName = target.LastName,
      EmailAddress = target.EmailAddress,
      IsAllowed = target.IsAllowed,
      SecurityRoleId = target.SecurityRoleId
    };
  }
}
