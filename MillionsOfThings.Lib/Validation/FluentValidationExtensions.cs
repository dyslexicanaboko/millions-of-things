using FluentValidation;
using MillionsOfThings.Lib.Exceptions;
using static MillionsOfThings.Lib.Exceptions.InvalidArgument;

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

    public static IRuleBuilderOptions<T, string> TestStringLength<T>(
      this IRuleBuilderInitial<T, string> rule,
      string argument,
      int min,
      int max)
      =>
        rule
        .MinimumLength(min)
        .MaximumLength(max)
        .WithMessageAndErrorCode(StringLength(argument, min, max));
  }
}
