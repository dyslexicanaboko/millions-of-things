using Npgsql;
using System.Data;

namespace MillionsOfThings.Lib.DataAccess
{
  public interface IRepository<T>
    : IRepository
    where T : class, new()
  {
    Task<T?> Select(int id);

    Task<IEnumerable<T>> SelectAll();

    Task<int> Insert(T entity);

    Task Update(T entity);
  }

  public interface IRepository
    : IDisposable
  {
    //This exists to unify all repositories as being "a type of" repository
    //Exposes methods for working with transactions, so that repositories can work together in a single transaction
    Task BeginTransaction();

    Task CommitTransaction();

    Task RollbackTransaction();

    void JoinExistingTransaction(IDbTransaction transaction);
  }
}
