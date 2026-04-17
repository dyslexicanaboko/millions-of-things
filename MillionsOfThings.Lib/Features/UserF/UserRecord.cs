namespace MillionsOfThings.Lib.Features.UserF;

public record UserRecord
{
  public int UserId { get; init; }

  public bool IsAllowed { get; init; }

  public required string Username { get; init; }

  //This is only populated for create and for authorization
  public required string Password { get; init; }

  public required string FirstName { get; init; }

  public required string LastName { get; init; }

  public required string EmailAddress { get; init; }

  public DateTime CreatedOn { get; init; }
}