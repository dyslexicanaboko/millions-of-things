namespace MillionsOfThings.Lib.Features.TaskFeature;

public record TaskRecord
{
  public required int TaskId { get; init; }

  public required int UserId { get; init; }

  public int? CategoryId { get; init; }

  public required string Description { get; init; }

  public required bool IsFinished { get; init; }

  public DateTime? FinishedOn { get; init; }

  public required DateTime CreatedOn { get; init; }

  public DateTime? ModifiedOn { get; init; }
}