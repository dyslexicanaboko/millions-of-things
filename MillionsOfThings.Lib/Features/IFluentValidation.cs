using FluentValidation.Results;

namespace MillionsOfThings.Lib.Features
{
  public interface IFluentValidation<in TEntity>
  {
    ValidationResult Validate(TEntity instance);
  }
}
