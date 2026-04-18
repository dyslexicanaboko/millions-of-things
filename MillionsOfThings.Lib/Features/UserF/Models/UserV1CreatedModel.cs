namespace MillionsOfThings.Lib.Features.UserF.Models;

public class UserV1CreatedModel
{
  public UserV1CreatedModel(UserEntity target)
  {
    UserId = target.UserId;

    Username = target.Username;
  }

  public int UserId { get; set; }

  public string Username { get; set; }

  public string Password { get; set; }

  public string FirstName { get; set; }

  public string LastName { get; set; }

  public string EmailAddress { get; set; }
}