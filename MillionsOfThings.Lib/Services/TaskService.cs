using Microsoft.AspNetCore.JsonPatch;
using MillionsOfThings.Lib.DataAccess;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Mappers;
using MillionsOfThings.Lib.Models.Client;
using MillionsOfThings.Lib.Services.Utility;
using MillionsOfThings.Lib.Validation;

namespace MillionsOfThings.Lib.Services
{
  public class TaskService
    : BaseService, ITaskService
  {
    private readonly ITaskRepository _repository;
    private readonly ITaskValidation _validation;
    private readonly ITaskMapper _mapper;

    public TaskService(
      ITaskRepository repository,
      ITaskValidation validation,
      ITaskMapper mapper)
    {
      _repository = repository;
      _validation = validation;
      _mapper = mapper;
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

    public async Task EditPartial(int userId, int taskId, JsonPatchDocument<TaskV1PatchModel> patchDoc)
    {
      //Get the existing object from the DB
      var db = await Get(taskId);

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
          object? finishedOn = (bool)prop.GetValue(entity)! == true ? DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified) : null;
          
          lst.Add(new UpdateInstruction(nameof(TaskEntity.FinishedOn), finishedOn));
        }
      });

      await _repository.Using(x => x.UpdatePartial(taskId, instructions));
    }

    public async Task Edit(TaskEntity entity)
    {
      Validations.IsValid(_validation, entity, nameof(entity));

      await _repository.Using(x => x.Update(entity));
    }

    public async Task Remove(int taskId)
    {
      Validations.IsGreaterThanZero(taskId, nameof(taskId));

      await _repository.Using(x => x.Delete(taskId));
    }
  }
}
