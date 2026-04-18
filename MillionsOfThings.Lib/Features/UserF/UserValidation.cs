using FluentValidation;

namespace MillionsOfThings.Lib.Features.UserF;

public interface IUserValidation
    : IFluentValidation<UserEntity>
{
}

public class UserValidation
  : AbstractValidator<UserEntity>, IUserValidation
{
  public UserValidation()
  {
    RuleFor(r => r.FirstName);

    RuleFor(r => r.LastName);

    RuleFor(r => r.EmailAddress);
  }
}