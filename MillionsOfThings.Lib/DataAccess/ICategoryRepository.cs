using MillionsOfThings.Lib.Entities;

namespace MillionsOfThings.Lib.DataAccess;

public interface ICategoryRepository : IRepository
{
  Task<CategoryEntity?> Select(int userId, int categoryId);

  Task<IEnumerable<CategoryEntity>> SelectAll(int userId);

  Task<int> Insert(CategoryEntity entity);

  Task Update(CategoryEntity entity);

  Task<int> Delete(int userId, int categoryId);

  Task<int> UsageCount(int userId, int categoryId);

  Task<int> DetachFromTasks(int userId, int categoryId);

  Task<bool> Exists(int userId, int categoryId);
  
  Task<bool> Exists(int userId, string name);
}
