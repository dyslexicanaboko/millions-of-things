namespace MillionsOfThings.Lib.DataAccess
{
  public static class RepositoryExtensions
  {
    public static async Task<TReturn> Using<TRepo, TReturn>(this TRepo repo, Func<TRepo, Task<TReturn>> method)
      where TRepo : IRepository
    {
      using (repo)
      {
        return await method(repo);
      }
    }

    public static async Task Using<TRepo>(this TRepo repo, Func<TRepo, Task> method)
      where TRepo : IRepository
    {
      using (repo)
      {
        await method(repo);
      }
    }
  }
}
