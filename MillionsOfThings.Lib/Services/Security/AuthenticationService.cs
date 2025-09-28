using MillionsOfThings.Lib.DataAccess;
using MillionsOfThings.Lib.DataAccess.Security;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Exceptions;
using Crypto = BCrypt.Net.BCrypt; //Naming it so it's clear a 3rd party is being used

namespace MillionsOfThings.Lib.Services.Security
{
  public class AuthenticationService
    : IAuthenticationService
  {
    private readonly IUserRepository _repoUser;

    public AuthenticationService(
      IUserRepository repoUser)
      => _repoUser = repoUser;

    public async Task<UserEntity> Authenticate(string username, string password)
    {
      //Direct Repo access on purpose to have a separation of concerns between the UserService and Authentication
      //The password is needed only in this situation.
      var entity = await _repoUser.Using(x => x.Select(username));

      //If user isn't found
      if (entity == null)
      {
        //throw exception about user not being found
        throw NotFound.UserCredentials();
      }

      //Explicitly denied access
      if (!entity.IsAllowed) throw Unauthorized.FailedAuthentication();

      if (!IsPasswordValid(password, entity.Password)) throw Unauthorized.InvalidPassword();

      //There is no conceivable scenario where the encrypted password is needed, just blank it out
      entity.Password = string.Empty;

      return entity;
    }

    //Will need this later for creating users
    private static string HashPassword(string plainTextPassword)
    {
      var salt = Crypto.GenerateSalt(12);

      var hashedPassword = Crypto.HashPassword(plainTextPassword, salt);

      return hashedPassword;
    }

    private static bool IsPasswordValid(string password, string correctHash)
    {
      var isValid = Crypto.Verify(password, correctHash);

      return isValid;
    }
  }
}
