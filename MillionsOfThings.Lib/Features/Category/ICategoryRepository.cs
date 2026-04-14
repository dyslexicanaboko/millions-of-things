namespace MillionsOfThings.Lib.Features.Category;

public interface ICategoryRepository : IRepository
{
  Task<CategoryRecord?> Select(int userId, int categoryId);

  Task<List<CategoryRecord>> SelectAll(int userId);

  Task<int> Insert(CategoryRecord entity);

  Task Update(CategoryRecord entity);

  Task<(bool, int)> Delete(int userId, int categoryId);

  Task<int> UsageCount(int userId, int categoryId);

  //Task<int> DetachFromTasks(int userId, int categoryId);

  Task<bool> Exists(int userId, int categoryId);
  
  Task<bool> Exists(int userId, string name);
}
