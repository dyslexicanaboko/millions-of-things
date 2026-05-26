using MillionsOfThings.Lib.Features.SecurityFeature.Constants;

namespace MillionsOfThings.Lib.Features.SecurityFeature.Authenticated;

public class ClaimsUserModel(ILookup<string, string> claims)
{
  public int UserId => Convert.ToInt32(claims[JwtClaims.UserId].Single());

  public string FullName => claims[JwtClaims.FullName].Single();

  public string Username => claims[JwtClaims.Username].Single();

  public string Role => claims[JwtClaims.Role].Single();

  public string[] Permissions  => claims[JwtClaims.Permission].ToArray();
}