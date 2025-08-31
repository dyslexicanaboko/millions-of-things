using MillionsOfThings.Lib.DataAccess;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Validation;

namespace MillionsOfThings.Lib.Services
{
  public class TaskService
    : ITaskService
  {
    private readonly ITaskRepository _repository;
    private readonly ITaskValidation _validation;

    public TaskService(
      ITaskRepository repository,
      ITaskValidation validation)
    {
      _repository = repository;
      _validation = validation;
    }

    public async Task<TaskEntity?> Get(int taskId)
    {
      Validations.IsGreaterThanZero(taskId, nameof(taskId));

      var dbEntity = await _repository.Using(x => x.Select(taskId));

      return dbEntity;
    }

    public async Task<IList<TaskEntity>> GetAll(int userId)
    {
      Validations.ThrowOnError(
        () => Validations.IsUserIdValid(userId, false));

      var lst = (await _repository
        .Using(x => x.SelectAll(userId)))
        .ToList();

      return lst;
    }

    public async Task<TaskEntity> Add(TaskEntity? entity)
    {
      Validations.IsValid(_validation, entity, nameof(entity));

      entity.TaskId = await _repository.Using(x => x.Insert(entity));

      return entity;
    }

    public async Task Edit(TaskEntity entity)
    {
      Validations.IsNotNull(entity, nameof(entity));

      await _repository.Using(x => x.Update(entity));
    }

    public async Task Remove(int taskId)
    {
      Validations.IsGreaterThanZero(taskId, nameof(taskId));

      await _repository.Using(x => x.Delete(taskId));
    }
  }
}
