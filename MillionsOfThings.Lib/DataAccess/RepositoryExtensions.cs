namespace MillionsOfThings.Lib.DataAccess
{
  /* This idea has run its course, with the inclusion of newer syntactical sugar (await using var con...),
   * this is no longer necessary. Additionally, with the change in attitude about
   * how the repository layer should be structured, such as using feature based
   * development instead of table based development, this is less useful.
   */
  public static class RepositoryExtensions
  {
    //public static async Task<TReturn> Using<TRepo, TReturn>(this TRepo repo, Func<TRepo, Task<TReturn>> method)
    //  where TRepo : IRepository
    //{
    //  using (repo)
    //  {
    //    return await method(repo);
    //  }
    //}

    //public static async Task Using<TRepo>(this TRepo repo, Func<TRepo, Task> method)
    //  where TRepo : IRepository
    //{
    //  using (repo)
    //  {
    //    await method(repo);
    //  }
    //}
  }
}
