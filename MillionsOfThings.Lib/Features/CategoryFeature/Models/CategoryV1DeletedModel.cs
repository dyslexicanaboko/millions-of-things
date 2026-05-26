namespace MillionsOfThings.Lib.Features.CategoryFeature.Models
{
  public class CategoryV1DeletedModel(CategoryRemovalResult result)
  {
    public bool IsSuccessful { get; set; } = result.IsSuccessful;

    public int AffectedTasks { get; set; } = result.AffectedTasks;
  }
}
