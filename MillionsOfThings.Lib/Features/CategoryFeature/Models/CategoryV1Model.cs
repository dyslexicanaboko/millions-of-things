namespace MillionsOfThings.Lib.Features.CategoryFeature.Models
{
  public class CategoryV1Model
  {
    public CategoryV1Model()
    {
    }

    public CategoryV1Model(ICategory target)
    {
      CategoryId = target.CategoryId;
      UserId = target.UserId;
      Name = target.Name;
    }

    public int CategoryId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; }
  }
}
