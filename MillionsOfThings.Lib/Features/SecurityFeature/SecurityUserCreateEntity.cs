using MillionsOfThings.Lib.Features.UserFeature.Models;

namespace MillionsOfThings.Lib.Features.SecurityFeature;

public class SecurityUserCreateEntity
{
  public SecurityUserCreateEntity(
    UserV1CreateModel model,
    bool isAllowed,
    string password,
    Guid securityRoleId)
  {
    Username = model.Username;
    FirstName = model.FirstName;
    LastName = model.LastName;
    EmailAddress = model.EmailAddress;
    IsAllowed = isAllowed;
    Password = password;
    SecurityRoleId = securityRoleId;
  }

  public int UserId { get; set; }

  public bool IsAllowed { get; set; }

  public string Username { get; set; }
  
  public string Password { get; set; }

  public string FirstName { get; set; }

  public string LastName { get; set; }

  public string EmailAddress { get; set; }

  public Guid SecurityRoleId { get; set; }
}