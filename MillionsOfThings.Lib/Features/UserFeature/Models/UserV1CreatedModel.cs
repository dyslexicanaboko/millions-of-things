using MillionsOfThings.Lib.Features.SecurityFeature;

namespace MillionsOfThings.Lib.Features.UserFeature.Models;

public class UserV1CreatedModel
{
  public UserV1CreatedModel(UserEntity target)
  {
    UserId = target.UserId;
  }

  public UserV1CreatedModel(SecurityUserCreateEntity target)
  {
    UserId = target.UserId;
  }

  public int UserId { get; set; }

  //public string Username { get; set; }

  //public string FirstName { get; set; }

  //public string LastName { get; set; }

  //public string EmailAddress { get; set; }

  //public string Role { get; set; }
}