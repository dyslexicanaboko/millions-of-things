using MillionsOfThings.Lib.Entities;
using Npgsql;

namespace MillionsOfThings.Lib.DataAccess;

public interface ICategoryRepository : IRepository<CategoryEntity>
{
  Task<CategoryEntity?> Select(int categoryId);

  Task<IEnumerable<CategoryEntity>> SelectAll(int userId);

  Task<int> Insert(CategoryEntity entity);

  Task Update(CategoryEntity entity);

  Task Delete(int categoryId);

  Task<bool> Exists(int userId, int categoryId);
  
  Task<bool> Exists(int userId, string name);

  void Dispose();

  void SetTransaction(NpgsqlTransaction transaction);
}
