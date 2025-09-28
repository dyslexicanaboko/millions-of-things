using MillionsOfThings.Lib.Models.Client;

namespace MillionsOfThings.Lib.Services.Security;

public interface ITokenService
{
  Task<JwtTokenV1Model> GetToken(AuthenticationV1PostModel model, string ipAddress);

  Task<JwtTokenV1Model> GetToken(RefreshTokenV1PostModel model, string ipAddress);
}
