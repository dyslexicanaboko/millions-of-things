namespace MillionsOfThings.Lib.Features.TaskFeature.Models
{
  public class TaskV1CreateModel
  {
    public TaskV1CreateModel()
    {
      
    }

    public int? CategoryId { get; set; }

    public string Description { get; set; }
  }
}
