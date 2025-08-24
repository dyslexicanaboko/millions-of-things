using MillionsOfThings.Lib.Entities;

namespace MillionsOfThings.Lib.DataAccess;

public interface ITaskRepository : IRepository
{
  Task<IEnumerable<TaskEntity>> SelectByUserId(int userId);

  Task<TaskEntity?> Select(int taskId);

  Task<IEnumerable<TaskEntity>> SelectAll();

  Task<int> Insert(TaskEntity entity);

  Task Update(TaskEntity entity);

  Task Delete(int taskId);
}
