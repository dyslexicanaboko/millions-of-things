using System.Security.Cryptography;
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

    public string GenerateHashedTemporaryPassword()
      => HashPassword(GenerateTemporaryPassword(16)); //TODO: Make the length of the temporary password a configuration setting.

    private static string GenerateTemporaryPassword(int length)
    {
      const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
      var bytes = RandomNumberGenerator.GetBytes(length);
      var chars = new char[length];

      for (var i = 0; i < length; i++)
      {
        chars[i] = validChars[bytes[i] % validChars.Length];
      }

      return new string(chars);
    }
  }
}
