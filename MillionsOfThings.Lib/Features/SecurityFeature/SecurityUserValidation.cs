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
  public SecurityUserValidation(ISecurityUserRepository repo)
  {
    RuleFor(r => r.FirstName)
      .NotEmpty()
      .WithMessageAndErrorCode(InvalidArgument.Empty(nameof(SecurityUserCreateEntity.FirstName)))
      .DependentRules(() =>
      {
        RuleFor(r => r.FirstName)
          .TestStringLength(nameof(SecurityUserCreateEntity.FirstName), 1, 50);
      });

    RuleFor(r => r.LastName)
      .NotEmpty()
      .WithMessageAndErrorCode(InvalidArgument.Empty(nameof(SecurityUserCreateEntity.LastName)))
      .DependentRules(() =>
      {
        RuleFor(r => r.LastName)
          .TestStringLength(nameof(SecurityUserCreateEntity.LastName), 1, 50);
      });
    
    RuleFor(r => r.EmailAddress)
      .NotEmpty()
      .WithMessageAndErrorCode(InvalidArgument.Empty(nameof(SecurityUserCreateEntity.EmailAddress)))
      .DependentRules(() =>
      {
        RuleFor(r => r.EmailAddress)
          .TestStringLength(nameof(SecurityUserCreateEntity.EmailAddress), 5, 100);

        RuleFor(r => r.EmailAddress)
          .EmailAddress()
          .WithMessageAndErrorCode(InvalidArgument.InvalidEmailAddress(nameof(SecurityUserCreateEntity.EmailAddress)));
      }).DependentRules(() =>
      {
        RuleFor(r => r.EmailAddress)
          .CustomAsync(async (email, context, cancellationToken) =>
          {
            //TODO: Implement cancellation tokens everywhere possible
            var exists = await repo.DoesEmailAddressExist(email, cancellationToken);

            if (exists)
            {
              context.AddFailure(nameof(SecurityUserCreateEntity.EmailAddress), "Email address is already in use.");
            }
          });
      });

    RuleFor(r => r.Username)
      .NotEmpty()
      .WithMessageAndErrorCode(InvalidArgument.Empty(nameof(SecurityUserCreateEntity.Username)))
      .DependentRules(() =>
      {
        RuleFor(r => r.Username)
          .TestStringLength(nameof(SecurityUserCreateEntity.Username), 1, 20);
      })
      .DependentRules(() =>
      {
        RuleFor(r => r.Username)
          .CustomAsync(async (username, context, cancellationToken) =>
          {
            //TODO: Implement cancellation tokens everywhere possible
            var exists = await repo.DoesUsernameExist(username, cancellationToken);

            if (exists)
            {
              context.AddFailure(nameof(SecurityUserCreateEntity.Username), "Username is already in use.");
            }
          });
      });
  }
}