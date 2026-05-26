namespace MillionsOfThings.Lib.Features.TaskFeature.Models
{
  public class TaskV1PatchModel
  {
    public TaskV1PatchModel()
    {

    }

    public TaskV1PatchModel(ITask task)
    {
      CategoryId = task.CategoryId;
      Description = task.Description;
      IsFinished = task.IsFinished;
    }

    public int? CategoryId { get; set; }

    public string Description { get; set; }

    public bool IsFinished { get; set; }
  }
}
