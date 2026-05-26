namespace MillionsOfThings.Lib.Features.SecurityFeature;

public record SecurityUserRecord
{
  public int UserId { get; init; }

  public bool IsAllowed { get; init; }

  public required string Username { get; init; }

  public required string Password { get; set; }

  public required string FirstName { get; init; }

  public required string LastName { get; init; }

  public required Guid SecurityRoleId { get; init; }
  
  public required string Role { get; init; }
}