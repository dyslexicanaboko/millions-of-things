using MillionsOfThings.Lib.Features.UserF.Models;

namespace MillionsOfThings.Lib.Features.UserF;

//NOTE: Password is purposely not exposed in this object
public class UserEntity
{
  public UserEntity()
  {

  }

  public UserEntity(UserRecord target)
  {
    UserId = target.UserId;
    IsAllowed = target.IsAllowed;
    Username = target.Username;
    FirstName = target.FirstName;
    LastName = target.LastName;
    EmailAddress = target.EmailAddress;
    CreatedOn = target.CreatedOn;
  }

  public UserEntity(UserV1CreateModel model)
  {
    //TODO: I will be doing this soon
  }

  public int UserId { get; set; }

  public bool IsAllowed { get; set; }

  public string Username { get; set; }
  
  public string FirstName { get; set; }
  
  public string LastName { get; set; }
  
  public string EmailAddress { get; set; }
  
  public DateTime CreatedOn { get; set; }
}