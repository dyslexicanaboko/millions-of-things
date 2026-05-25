using MillionsOfThings.Lib.Features.Security.Models;

namespace MillionsOfThings.Lib.Features.Security;

public interface ITokenManager
{
  Task<JwtTokenV1Model> GetToken(AuthenticationV1PostModel model, string ipAddress);

  Task<JwtTokenV1Model> GetToken(RefreshTokenV1PostModel model, string ipAddress);
}
