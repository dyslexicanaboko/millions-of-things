namespace MillionsOfThings.Lib.Mappers
{
  public abstract class BaseMapper
  {
    protected static IList<TOutput> ToList<TInput, TOutput>(IList<TInput>? target, Func<TInput?, TOutput?> mapper)
      where TInput : class, new()
      where TOutput : class //The new() constraint does not matter for outputs since the mapper handles it.
    {
      if (target == null || !target.Any()) return new List<TOutput>();

      var lst = target.Select(mapper).ToList();

      return lst!;
    }

    protected static IList<TOutput> ToList<TInput, TOutput>(int userId, IList<TInput>? target, Func<int, TInput?, TOutput?> mapper)
      where TInput : class, new()
      where TOutput : class, new() //The new() constraint does not matter for outputs since the mapper handles it.
    {
      if (target == null || !target.Any()) return new List<TOutput>();

      var lst = target.Select(x => mapper(userId, x)).ToList();

      return lst!;
    }

    //NOTE: There isn't a simple way to enforce that TRecordInput is a record class, but this method is only intended for record classes for now.
    protected static IList<TOutput> ToListR<TRecordInput, TOutput>(IList<TRecordInput>? target, Func<TRecordInput?, TOutput?> mapper)
      where TRecordInput : class //Record classes cannot use the new() constraint.
      where TOutput : class //The new() constraint does not matter for outputs since the mapper handles it.
    {
      if (target == null || !target.Any()) return new List<TOutput>();

      var lst = target.Select(mapper).ToList();

      return lst!;
    }
  }
}
