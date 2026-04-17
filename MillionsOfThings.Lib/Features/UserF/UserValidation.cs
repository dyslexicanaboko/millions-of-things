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
    RuleFor(r => r.Firstname);

    RuleFor(r => r.Lastname);

    RuleFor(r => r.Emailaddress);
  }
}