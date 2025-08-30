using MillionsOfThings.Lib.Entities;

namespace MillionsOfThings.Lib.Models
{
  public class TaskV1Model
    : ITask
  {
    public int TaskId { get; set; }

    public int UserId { get; set; }

    public int? CategoryId { get; set; }

    public string Description { get; set; }

    public bool IsFinished { get; set; }

    public DateTime? FinishedOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
  }
}
