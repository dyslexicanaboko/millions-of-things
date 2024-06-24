using FluentValidation.Results;

namespace MillionsOfThings.Lib.Validation
{
  public interface IFluentValidation<in TEntity>
  {
    ValidationResult Validate(TEntity instance);
  }
}
