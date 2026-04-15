using MillionsOfThings.Lib.Features.Security.Models;

namespace MillionsOfThings.Lib.Features.Security;

public class UserEntity
{
  public UserEntity()
  {

  }

  public UserEntity(UserV1CreateModel target)
  {
    //TODO: Later I have to introduce the other properties.
    //Name = target.Name;
  }

  public int UserId { get; set; }

  public bool IsAllowed { get; set; }

  public string Username { get; set; }

  //This is only populated for create and for authorization
  public string Password { get; set; }

  public DateTime CreateOnUtc { get; set; }
}