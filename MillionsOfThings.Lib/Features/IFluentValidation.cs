using FluentValidation.Results;

namespace MillionsOfThings.Lib.Features;

//The contracts here have to match what is exposed from AbstractValidator<TEntity> in FluentValidation.
public interface IFluentValidation<in TEntity>
{
  ValidationResult Validate(TEntity instance);
    
  Task<ValidationResult> ValidateAsync(TEntity instance, CancellationToken cancellation = default);
}