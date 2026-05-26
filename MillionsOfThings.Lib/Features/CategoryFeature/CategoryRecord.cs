namespace MillionsOfThings.Lib.Features.CategoryFeature;

public record CategoryRecord
{
  public required int CategoryId { get; init; }

  public required int UserId { get; init; }

  public required string Name { get; init; }
  
  public required DateTime CreatedOn { get; init; }

  public DateTime? ModifiedOn { get; init; }
}