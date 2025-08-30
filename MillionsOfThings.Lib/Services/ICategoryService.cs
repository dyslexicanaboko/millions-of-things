using MillionsOfThings.Lib.Entities;

namespace MillionsOfThings.Lib.Services;

public interface ICategoryService
{
  Task<CategoryEntity?> GetCategory(int categoryId);

  Task<CategoryEntity> Add(CategoryEntity? entity);

  Task Edit(CategoryEntity entity);

  Task Remove(int categoryId);
}
