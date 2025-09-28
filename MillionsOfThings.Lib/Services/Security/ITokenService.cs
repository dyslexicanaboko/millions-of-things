using MillionsOfThings.Lib.Models.Client;

namespace MillionsOfThings.Lib.Services.Security;

public interface ITokenService
{
  Task<string> GetToken(AuthenticationV1PostModel model, string ipAddress);

  Task<string> GetToken(RefreshTokenV1PostModel model, string ipAddress);
}
