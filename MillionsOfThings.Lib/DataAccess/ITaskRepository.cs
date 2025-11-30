using MillionsOfThings.Lib.Records;
using MillionsOfThings.Lib.Services.Utility;

namespace MillionsOfThings.Lib.DataAccess;

public interface ITaskRepository : IRepository
{
  Task<IList<TaskRecord>> SelectAll(int userId);

  Task<TaskRecord?> Select(int taskId, int userId);

  Task<IList<TaskRecord>> SelectAll();

  Task<int> Insert(TaskRecord entity);

  Task Update(TaskRecord entity);

  Task UpdatePartial(int userId, int taskId, IList<UpdateInstruction> instructions);

  Task Delete(int userId, int taskId);
}
