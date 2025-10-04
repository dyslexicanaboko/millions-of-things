using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Services.Utility;

namespace MillionsOfThings.Lib.DataAccess;

public interface ITaskRepository : IRepository
{
  Task<IEnumerable<TaskEntity>> SelectAll(int userId);

  Task<TaskEntity?> Select(int taskId, int userId);

  Task<IEnumerable<TaskEntity>> SelectAll();

  Task<int> Insert(TaskEntity entity);

  Task Update(TaskEntity entity);

  Task UpdatePartial(int userId, int taskId, IList<UpdateInstruction> instructions);

  Task Delete(int userId, int taskId);
}
