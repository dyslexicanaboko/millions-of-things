using FluentValidation;
using MillionsOfThings.Lib.Exceptions;

namespace MillionsOfThings.Lib.Features.SecurityFeature;

public interface ISecurityUserValidation
  : IFluentValidation<SecurityUserCreateEntity>
{
}

public class SecurityUserValidation
  : AbstractValidator<SecurityUserCreateEntity>, ISecurityUserValidation
{
  /* TODO:
      1. Make sure username isn't in use
      2. Make sure email address isn't in use
      3. Make sure password is strong enough
      4. Make sure incoming security role is valid (string)
   */
  public SecurityUserValidation()
  {
    RuleFor(r => r.FirstName)
      .NotEmpty()
      .WithMessageAndErrorCode(InvalidArgument.Empty(nameof(SecurityUserCreateEntity.FirstName)));

    RuleFor(r => r.LastName)
      .NotEmpty()
      .WithMessageAndErrorCode(InvalidArgument.Empty(nameof(SecurityUserCreateEntity.LastName)));

    RuleFor(r => r.EmailAddress)
      .NotEmpty()
      .WithMessageAndErrorCode(InvalidArgument.Empty(nameof(SecurityUserCreateEntity.EmailAddress)));

    RuleFor(r => r.FirstName)
      .TestStringLength(nameof(SecurityUserCreateEntity.FirstName), 1, 50);

    RuleFor(r => r.LastName)
      .TestStringLength(nameof(SecurityUserCreateEntity.LastName), 1, 50);

    RuleFor(r => r.EmailAddress)
      .TestStringLength(nameof(SecurityUserCreateEntity.EmailAddress), 1, 100);
  }
}