using MillionsOfThings.Lib.Features.UserFeature.Models;

namespace MillionsOfThings.Lib.Features.SecurityFeature;

public class SecurityUserMapper
  : BaseMapper, ISecurityUserMapper
{
  public SecurityUserCreateEntity? ToEntity(UserV1CreateModel? target)
  {
    if (target == null) return null;
    
    return new SecurityUserCreateEntity(target);
  }

  public SecurityUserCreateRecord ToRecord(SecurityUserCreateEntity target)
  {
    //TODO: How do I handle the security_role_id here?
    return new SecurityUserCreateRecord
    {
      Username = target.Username,
      Password = target.Password,
      FirstName = target.FirstName,
      LastName = target.LastName,
      EmailAddress = target.EmailAddress,
      IsAllowed = target.IsAllowed
    };
  }
}
