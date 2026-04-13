namespace MillionsOfThings.Lib.Features.Security;

public interface IAuthenticationService
{
  Task<UserEntity> Authenticate(string username, string password);
}
