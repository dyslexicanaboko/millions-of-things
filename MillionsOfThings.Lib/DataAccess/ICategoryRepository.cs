using MillionsOfThings.Lib.Entities;
using Npgsql;

namespace MillionsOfThings.Lib.DataAccess;

public interface ICategoryRepository : IRepository<CategoryEntity>
{
  Task<CategoryEntity?> Select(int categoryId);

  Task<IEnumerable<CategoryEntity>> SelectAll();

  Task<int> Insert(CategoryEntity entity);

  Task Update(CategoryEntity entity);

  Task Delete(int categoryId);

  Task<bool> Exists(CategoryEntity entity);

  void Dispose();

  void SetTransaction(NpgsqlTransaction transaction);
}
