namespace MillionsOfThings.Lib.Features.Security;

public record RefreshTokenRecord
{
  public Guid RefreshTokenId { get; init; }

  public int UserId { get; init; }

  public required string Token { get; init; }

  public DateTime ExpiresOn { get; init; }

  public DateTime CreatedOn { get; init; }

  public required string CreatedByIp { get; init; }
}