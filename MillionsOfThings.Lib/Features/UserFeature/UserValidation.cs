using FluentValidation;
using MillionsOfThings.Lib.Exceptions;

namespace MillionsOfThings.Lib.Features.UserFeature;

public interface IUserValidation
    : IFluentValidation<UserEntity>
{
}

public class UserValidation
  : AbstractValidator<UserEntity>, IUserValidation
{
  //TODO: Should I go as far as to check for existence here?
  public UserValidation()
  {
    RuleFor(r => r.FirstName)
      .NotEmpty()
      .WithMessageAndErrorCode(InvalidArgument.Empty(nameof(UserEntity.FirstName)));

    RuleFor(r => r.LastName)
      .NotEmpty()
      .WithMessageAndErrorCode(InvalidArgument.Empty(nameof(UserEntity.LastName)));

    RuleFor(r => r.EmailAddress)
      .NotEmpty()
      .WithMessageAndErrorCode(InvalidArgument.Empty(nameof(UserEntity.EmailAddress)));

    RuleFor(r => r.FirstName)
      .TestStringLength(nameof(UserEntity.FirstName), 1, 50);

    RuleFor(r => r.LastName)
      .TestStringLength(nameof(UserEntity.LastName), 1, 50);

    RuleFor(r => r.EmailAddress)
      .TestStringLength(nameof(UserEntity.EmailAddress), 1, 100);
  }
}