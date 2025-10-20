namespace MillionsOfThings.Lib.Results
{
  public class CategoryRemovalResult(bool isSuccessful, int affectedTasks)
  {
    public bool IsSuccessful { get; set; } = isSuccessful;

    public int AffectedTasks { get; set; } = affectedTasks;
  }
}
