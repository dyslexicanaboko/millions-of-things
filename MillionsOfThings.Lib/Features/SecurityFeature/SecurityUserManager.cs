using MillionsOfThings.Lib.Exceptions;
using MillionsOfThings.Lib.Features.SecurityFeature.Authenticated;
using MillionsOfThings.Lib.Features.SecurityFeature.Constants;
using MillionsOfThings.Lib.Features.UserFeature.Models;

namespace MillionsOfThings.Lib.Features.SecurityFeature;

public interface ISecurityUserManager
{
  Task<SecurityUserCreateEntity> Add(
    ClaimsUserModel currentUser,
    UserV1CreateModel? user);
}

public class SecurityUserManager(
  ISecurityUserRepository repository,
  ISecurityUserValidation validation,
  ISecurityUserMapper mapper,
  ICryptographyService cryptographyService)
  : BaseManager, ISecurityUserManager
{
  public async Task<SecurityUserCreateEntity> Add(
    ClaimsUserModel currentUser,
    UserV1CreateModel? user)
  {
    Validations.IsNotNull(user, nameof(user));
    //Red test first - Standard user cannot do this
    //HasFullPermission(currentUser);

    //For now, going to check the Security Role here until I can think of a better way to do it.
    var roleId = await ValidateRole(user.Role);

    var entity = mapper.ToEntity(
      user, 
      false, 
      cryptographyService.GenerateHashedTemporaryPassword(),
      roleId);
    
    //TODO: How do I want to handle expected feedback?
    // Supposedly you should return the errors and not raise them as exceptions.
    validation.Validate(entity!);

    entity.UserId = await repository.Create(mapper.ToRecord(entity));

    return entity;
  }

  private async Task<Guid> ValidateRole(string role)
  {
    //Precheck before hitting the database
    if (!SecurityRoles.Contains(role))
    {
      throw InvalidArgument.InvalidRole(nameof(role));
    }

    var roleId = await repository.ReadSecurityRole(role);
    
    return roleId ?? throw InvalidArgument.InvalidRole(nameof(role));
  }
}