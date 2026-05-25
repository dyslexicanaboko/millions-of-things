namespace MillionsOfThings.Lib.Features.Security;

public interface ICryptographyService
{
  string HashPassword(string plainTextPassword);
  
  bool IsPasswordValid(string password, string correctHash);
}
