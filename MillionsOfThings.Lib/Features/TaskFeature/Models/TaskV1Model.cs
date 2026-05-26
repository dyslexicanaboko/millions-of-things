namespace MillionsOfThings.Lib.Features.TaskFeature.Models
{
  public class TaskV1Model
    : ITask
  {
    public TaskV1Model()
    {
      
    }

    public TaskV1Model(TaskEntity entity)
    {
      TaskId = entity.TaskId;
      UserId = entity.UserId;
      CategoryId = entity.CategoryId;
      Description = entity.Description;
      IsFinished = entity.IsFinished;
      FinishedOn = entity.FinishedOn;
      CreatedOn = entity.CreatedOn;
      ModifiedOn = entity.ModifiedOn;
    }

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
