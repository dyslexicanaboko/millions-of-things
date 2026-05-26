namespace MillionsOfThings.Lib.Features.CategoryFeature;

public interface ICategoryRepository : IRepository
{
  Task<CategoryRecord?> Read(int userId, int categoryId);

  Task<List<CategoryRecord>> ReadAll(int userId);

  Task<int> Create(CategoryRecord entity);

  Task Update(CategoryRecord entity);

  Task<(bool, int)> Delete(int userId, int categoryId);

  Task<int> UsageCount(int userId, int categoryId);

  //Task<int> DetachFromTasks(int userId, int categoryId);

  Task<bool> Exists(int userId, int categoryId);
  
  Task<bool> Exists(int userId, string name);
}
