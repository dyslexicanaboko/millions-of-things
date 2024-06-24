using FluentValidation;
using MillionsOfThings.Lib.Exceptions;

namespace MillionsOfThings.Lib.Validation
{
  public static class FluentValidationExtensions
  {
    public static IRuleBuilderOptions<T, TProperty> WithMessageAndErrorCode<T, TProperty>(
      this IRuleBuilderOptions<T, TProperty> rule,
      InvalidArgumentException exception)
      =>
        rule.WithMessage(exception.Message)
          .WithErrorCode(exception.ErrorCode.ToString());
  }
}
