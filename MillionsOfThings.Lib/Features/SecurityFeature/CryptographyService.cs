using Crypto = BCrypt.Net.BCrypt; //Naming it so it's clear a 3rd party is being used

namespace MillionsOfThings.Lib.Features.SecurityFeature
{
  public class CryptographyService
    : ICryptographyService
  {
    //Will need this later for creating users
    public string HashPassword(string plainTextPassword)
    {
      var salt = Crypto.GenerateSalt(12);

      var hashedPassword = Crypto.HashPassword(plainTextPassword, salt);

      return hashedPassword;
    }

    public bool IsPasswordValid(string password, string correctHash)
    {
      var isValid = Crypto.Verify(password, correctHash);

      return isValid;
    }
  }
}
