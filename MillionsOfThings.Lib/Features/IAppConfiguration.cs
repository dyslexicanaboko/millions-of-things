namespace MillionsOfThings.Lib.Features;

[ExcludeFromDiScan]
public interface IAppConfiguration
{
  string GetConnectionString();
}
