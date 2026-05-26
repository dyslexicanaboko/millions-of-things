using MillionsOfThings.Lib.Features.SecurityFeature.Models;

namespace MillionsOfThings.Lib.Features.SecurityFeature;

public interface ITokenManager
{
  Task<JwtTokenV1Model> GetToken(AuthenticationV1PostModel model, string ipAddress);

  Task<JwtTokenV1Model> GetToken(RefreshTokenV1PostModel model, string ipAddress);
}
