namespace MillionsOfThings.Lib.Features.UserF;

//NOTE: Password is purposely not exposed in this object
public record UserRecord
{
  public int UserId { get; init; }

  public bool IsAllowed { get; init; }

  public required string Username { get; init; }
  
  public required string FirstName { get; init; }

  public required string LastName { get; init; }

  public required string EmailAddress { get; init; }

  public DateTime CreatedOn { get; init; }
}