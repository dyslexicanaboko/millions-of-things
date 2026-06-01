using MillionsOfThings.Lib.Features.SecurityFeature.Authenticated;

namespace MillionsOfThings.Lib.Features.SecurityFeature;

public class SecurityUserManager(
  ISecurityUserRepository repository,
  ISecurityUserValidation validation,
  ISecurityUserMapper mapper)
  : BaseManager
{
  public async Task<SecurityUserCreateEntity> Add(
    ClaimsUserModel currentUser,
    SecurityUserCreateEntity? user)
  {
    //Red test first - Standard user cannot do this
    //HasFullPermission(currentUser);

    Validations.IsNotNull(user, nameof(user));
    
    //TODO: How do I want to handle expected feedback?
    // Supposedly you should return the errors and not raise them as exceptions.
    validation.Validate(user);

    user.UserId = await repository.Create(mapper.ToRecord(user));

    return user;
  }
}