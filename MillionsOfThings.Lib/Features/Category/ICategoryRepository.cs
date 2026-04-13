namespace MillionsOfThings.Lib.Features.Category;

public interface ICategoryRepository : IRepository
{
  Task<CategoryEntity?> Select(int userId, int categoryId);

  Task<List<CategoryEntity>> SelectAll(int userId);

  Task<int> Insert(CategoryEntity entity);

  Task Update(CategoryEntity entity);

  Task<(bool, int)> Delete(int userId, int categoryId);

  Task<int> UsageCount(int userId, int categoryId);

  //Task<int> DetachFromTasks(int userId, int categoryId);

  Task<bool> Exists(int userId, int categoryId);
  
  Task<bool> Exists(int userId, string name);
}
