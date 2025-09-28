using MillionsOfThings.Lib.Entities;

namespace MillionsOfThings.Lib.Services.Security;

public interface IAuthenticationService
{
  Task<UserEntity> Authenticate(string username, string password);
}
