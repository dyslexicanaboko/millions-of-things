namespace MillionsOfThings.Lib.Features.SecurityFeature;

public interface ICryptographyService
{
  string HashPassword(string plainTextPassword);
  
  bool IsPasswordValid(string password, string correctHash);
}
