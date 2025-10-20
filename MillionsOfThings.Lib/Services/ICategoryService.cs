using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Results;

namespace MillionsOfThings.Lib.Services;

public interface ICategoryService
{
  Task<CategoryEntity?> Get(int userId, int categoryId);

  Task<List<CategoryEntity>> GetAll(int userId);

  Task<CategoryEntity> Add(CategoryEntity? entity);

  Task Edit(CategoryEntity entity);

  Task<CategoryRemovalResult> Remove(int userId, int categoryId);
}
