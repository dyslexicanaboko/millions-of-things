using MillionsOfThings.Lib.Entities;

namespace MillionsOfThings.Lib.DataAccess;

public interface ITaskRepository
{
  TaskEntity Select(int taskId);

  IEnumerable<TaskEntity> SelectAll();

  int Insert(TaskEntity entity);

  void Update(TaskEntity entity);

  void Delete(TaskEntity entity);

  void Dispose();
}
