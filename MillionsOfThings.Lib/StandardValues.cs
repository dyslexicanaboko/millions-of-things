namespace MillionsOfThings.Lib;

public static class StandardValues
{
  public static DateTime GetUtcNow() 
    => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
}