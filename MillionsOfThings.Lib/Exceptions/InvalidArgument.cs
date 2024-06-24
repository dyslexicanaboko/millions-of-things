using BR = MillionsOfThings.Lib.Exceptions.ErrorCodes.BadRequest;

namespace MillionsOfThings.Lib.Exceptions
{
  public static class InvalidArgument
  {
    public static InvalidArgumentException Symbol = new(
      "symbol",
      "Symbol cannot be null, blank or white space.",
      BR.Symbol);

    public static InvalidArgumentException User = new("userId", "Invalid user.", BR.User);

    public static InvalidArgumentException MalformedModel()
      => new("model", "The provided model is malformed.", BR.MalformedModel);

    public static InvalidArgumentException Null(string argument)
      => new(argument, "The provided argument cannot be null.", BR.Null);

    public static InvalidArgumentException Empty(string argument)
      => new(argument, "The provided argument cannot be empty.", BR.Empty);

    public static InvalidArgumentException OutOfBounds(string argument, int lower, int upper)
      => new(argument, $"The provided argument must be between {lower} and {upper} inclusive.", BR.OutOfBounds);

    public static InvalidArgumentException OutOfBounds(string argument, string enumeration)
      => new(
        argument,
        $"The provided argument must be found in the {enumeration} enumeration.",
        BR.OutOfBoundsEnumeration);

    public static InvalidArgumentException NotGreaterThanZero(string argument)
      => new(argument, "The provided argument must be greater than zero.", BR.NotGreaterThanZero);

    public static InvalidArgumentException NotGreaterThanDateTimeMin(string argument)
      => new(
        argument,
        $"The provided argument must be greater than {DateTime.MinValue}.",
        BR.NotGreaterThanDateTimeMin);

    public static InvalidArgumentException StartDateGreaterThanEndDate(string argument)
      => new(
        argument,
        "The provided start date argument must be less than its end date.",
        BR.StartDateGreaterThanEndDate);

    public static InvalidArgumentException EndDateLessThanStartDate(string argument)
      => new(
        argument,
        "The provided end date argument must be greater than its start date.",
        BR.EndDateLessThanStartDate);
  }
}
