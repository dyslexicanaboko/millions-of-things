namespace MillionsOfThings.Lib.Features.UserF;

public record UserRecord
{
  public int UserId { get; init; }

  public bool IsAllowed { get; init; }

  public required string Username { get; init; }

  //This is only populated for create and for authorization
  public required string Password { get; init; }

  public DateTime CreateOnUtc { get; init; }
}