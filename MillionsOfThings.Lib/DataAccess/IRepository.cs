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

  /// <summary>
  /// This interface exists to unify all repositories as being "a type of" repository
  /// </summary>
  public interface IRepository
    //: IDisposable
  {
    //NOTE: 2025-12-28 I keep teetering back and forth on how to handle transactions in the data layer.
    //I have been on and off again. Right now I am off again.

    //Exposes methods for working with transactions, so that repositories can work together in a single transaction

    //Task BeginTransaction();

    //Task CommitTransaction();

    //Task RollbackTransaction();

    //void JoinExistingTransaction(IDbTransaction transaction);
  }
}
