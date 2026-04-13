namespace MillionsOfThings.Lib.Features.Category.Models
{
  public class CategoryV1CreatedModel
  {
    public CategoryV1CreatedModel()
    {

    }

    public CategoryV1CreatedModel(CategoryEntity entity)
    {
      CategoryId = entity.CategoryId;
      UserId = entity.UserId;
      Name = entity.Name;
    }

    public int CategoryId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
  }
}
