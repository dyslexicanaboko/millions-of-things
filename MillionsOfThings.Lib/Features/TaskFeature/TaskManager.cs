using Microsoft.AspNetCore.JsonPatch;
using MillionsOfThings.Lib.Features.TaskFeature.Models;
using MillionsOfThings.Lib.Utility;

namespace MillionsOfThings.Lib.Features.TaskFeature
{
  public class TaskManager
    : BaseManager, ITaskManager
  {
    private readonly ITaskRepository _repository;
    private readonly ITaskValidation _validation;
    private readonly ITaskMapper _mapper;

    public TaskManager(
      ITaskRepository repository,
      ITaskValidation validation,
      ITaskMapper mapper)
    {
      _repository = repository;
      _validation = validation;
      _mapper = mapper;
    }

    public async Task<TaskEntity?> Get(int userId, int taskId)
    {
      Validations.IsGreaterThanZero(taskId, nameof(taskId));

      return _mapper.ToEntity(await _repository.Read(taskId, userId));
    }

    public async Task<List<TaskEntity>> GetAll(int userId)
    {
      Validations.ThrowOnError(
        () => Validations.IsUserIdValid(userId, false));

      return _mapper.ToEntity(await _repository
        .ReadAll(userId));
    }

    public async Task<TaskEntity> Add(TaskEntity? entity)
    {
      Validations.IsValid(_validation, entity, nameof(entity));

      entity.CreatedOn = StandardValues.GetUtcNow();

      entity.TaskId = await _repository.Create(_mapper.ToRecord(entity));

      return entity;
    }

    public async Task EditPartial(int userId, int taskId, JsonPatchDocument<TaskV1PatchModel> patchDoc)
    {
      //Get the existing object from the DB
      var db = await Get(userId, taskId);

      //Either it doesn't exist or the user does not have access
      if (db == null) throw Exceptions.NotFound.Task(taskId);

      //Can only edit this task if it belongs to the requesting user
      if (db.UserId != userId) throw Exceptions.Forbidden.AccessDenied();

      //Get as a patch model
      var model = _mapper.ToPatchModel(db);

      //Apply incoming changes
      patchDoc.ApplyTo(model);

      //Change back to entity
      var entity = _mapper.ToEntity(userId, model);

      //Perform validation before continuing
      Validations.IsValid(_validation, entity, nameof(entity));

      var instructions = GetDifferences(db, entity, patchDoc, (lst, prop) =>
      {
        if (prop.Name == nameof(TaskEntity.IsFinished))
        {
          //The user cannot set this value, it's dependent on the `IsFinished` property
          object? finishedOn = (bool)prop.GetValue(entity)! == true ? StandardValues.GetUtcNow() : null;
          
          lst.Add(new UpdateInstruction(nameof(TaskEntity.FinishedOn), finishedOn));
        }
      });

      await _repository.UpdatePartial(userId, taskId, instructions);
    }

    public async Task Edit(TaskEntity entity)
    {
      Validations.IsValid(_validation, entity, nameof(entity));

      await _repository.Update(_mapper.ToRecord(entity));
    }

    public async Task Remove(int userId, int taskId)
    {
      Validations.IsGreaterThanZero(taskId, nameof(taskId));

      await _repository.Delete(userId, taskId);
    }
  }
}
