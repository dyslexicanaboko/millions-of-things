using Newtonsoft.Json;

namespace MillionsOfThings.Lib.Models.Client
{
  public class JwtTokenV1Model
  {
    public JwtTokenV1Model(string accessToken, int expiresIn)
    {
      AccessToken = accessToken;
      ExpiresIn = expiresIn;
    }

    [JsonProperty("access_token")] public string AccessToken { get; set; }

    [JsonProperty("expires_in")] public int ExpiresIn { get; set; }

    [JsonProperty("token_type")] public string TokenType { get; set; } = "Bearer";
  }
}
