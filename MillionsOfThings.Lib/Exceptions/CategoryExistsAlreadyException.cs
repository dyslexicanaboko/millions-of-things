using MillionsOfThings.Lib.Features.Category;

namespace MillionsOfThings.Lib.Exceptions
{
  public class CategoryExistsAlreadyException
    : EntityExistsAlreadyException
  {
    public CategoryExistsAlreadyException(CategoryEntity category)
      : base(GetMessage(category))
    {
    }

    private static string GetMessage(CategoryEntity category)
      => $"A category with name `{category.Name}` already exists for this user.";
  }
}
