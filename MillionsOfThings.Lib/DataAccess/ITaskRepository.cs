using MillionsOfThings.Lib.Entities;

namespace MillionsOfThings.Lib.DataAccess;

public interface ITaskRepository : IRepository
{
  TaskEntity? Select(int taskId);

  IEnumerable<TaskEntity> SelectByUserId(int userId);

  IEnumerable<TaskEntity> SelectAll();

  int Insert(TaskEntity entity);

  void Update(TaskEntity entity);

  void Delete(int taskId);
}
