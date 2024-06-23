namespace MillionsOfThings.Lib.Services;

[ExcludeFromDiScan]
public interface IAppConfiguration
{
  string GetConnectionString();
}
