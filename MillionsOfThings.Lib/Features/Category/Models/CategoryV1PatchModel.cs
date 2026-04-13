namespace MillionsOfThings.Lib.Features.Category.Models
{
  public class CategoryV1PatchModel
  {
    public CategoryV1PatchModel()
    {
      
    }

    public CategoryV1PatchModel(CategoryEntity entity)
    {
      Name = entity.Name;
    }

    public string Name { get; set; }
  }
}
