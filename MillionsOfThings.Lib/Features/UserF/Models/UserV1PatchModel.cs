namespace MillionsOfThings.Lib.Features.UserF.Models;

public class UserV1PatchModel
{
  public UserV1PatchModel()
  {

  }

  //TODO: I am not sure how I am going to handle password updates yet.
  public UserV1PatchModel(UserEntity target)
  {
    UserId = target.UserId;
    IsAllowed = target.IsAllowed;
    //Password = target.Password;
    FirstName = target.FirstName;
    LastName = target.LastName;
    EmailAddress = target.EmailAddress;
  }

  public int UserId { get; set; }
  
  public bool IsAllowed { get; set; }

  //public string Password { get; set; }

  public string FirstName { get; set; }

  public string LastName { get; set; }

  public string EmailAddress { get; set; }
}