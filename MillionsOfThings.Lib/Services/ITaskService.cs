using MillionsOfThings.Lib.Entities;

namespace MillionsOfThings.Lib.Services;

public interface ITaskService
{
  Task<TaskEntity?> Get(int taskId);

  Task<IList<TaskEntity>> GetAll(int userId);

  Task<TaskEntity> Add(TaskEntity? entity);

  Task Edit(TaskEntity entity);

  Task Remove(int taskId);
}
