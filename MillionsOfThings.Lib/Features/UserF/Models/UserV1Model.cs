namespace MillionsOfThings.Lib.Features.UserF.Models
{
  public class UserV1Model
  {
    public UserV1Model(UserEntity entity)
    {
      UserId = entity.UserId;
      Username = entity.Username;
      CreateOnUtc = entity.CreateOnUtc;
    }

    public int UserId { get; }

    public string Username { get; }

    public DateTime CreateOnUtc { get; }
  }
}
