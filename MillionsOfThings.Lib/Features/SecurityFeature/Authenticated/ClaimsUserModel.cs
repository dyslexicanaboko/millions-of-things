namespace MillionsOfThings.Lib.Features.SecurityFeature.Authenticated;

public class ClaimsUserModel(int userId, string fullName, string username, string role, string[] permissions)
{
  public int UserId => userId;

  public string FullName => fullName;

  public string Username => username;

  public string Role => role;

  public string[] Permissions  => permissions;
}