using MillionsOfThings.Lib.Entities;

namespace MillionsOfThings.Lib.Services;

public interface ITaskService
{
  Task<TaskEntity?> GetTask(int taskId);

  Task<IList<TaskEntity>> GetAllForUser(int userId);

  Task<TaskEntity> Add(TaskEntity? entity);

  Task Edit(TaskEntity entity);

  Task Remove(int taskId);
}
