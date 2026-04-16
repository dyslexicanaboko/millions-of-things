namespace MillionsOfThings.Lib.Features.Security;

public class RefreshTokenEntity
{
  public RefreshTokenEntity()
  {
    
  }

  public RefreshTokenEntity(RefreshTokenRecord record)
  {
    RefreshTokenId = record.RefreshTokenId;
    UserId = record.UserId;
    Token = record.Token;
    ExpiresOn = record.ExpiresOn;
    CreatedOn = record.CreatedOn;
    CreatedByIp = record.CreatedByIp;
  }

  public Guid RefreshTokenId { get; set; }

  public int UserId { get; set; }

  public string Token { get; set; }

  public DateTime ExpiresOn { get; set; }

  public DateTime CreatedOn { get; set; }

  public string CreatedByIp { get; set; }

  public bool IsExpired() => DateTime.UtcNow >= ExpiresOn;

  //Not convinced I need any of this
  //public DateTime? Revoked { get; set; }
  //public string RevokedByIp { get; set; }
  //public string ReplacedByToken { get; set; }
  //public string ReasonRevoked { get; set; }
  //public bool IsRevoked => Revoked != null;
  //public bool IsActive => !IsRevoked && !IsExpired;
}
