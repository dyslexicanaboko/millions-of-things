using MillionsOfThings.Lib.Entities;

namespace MillionsOfThings
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
