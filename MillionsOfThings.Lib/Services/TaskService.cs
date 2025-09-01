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
    : ITaskService
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

      var properties = entity.GetType().GetProperties();
      var instructions = new List<UpdateInstruction>(patchDoc.Operations.Count);

      foreach (var op in patchDoc.Operations)
      {
        //For each item in the operations list (which is what was explicitly provided)
        //Match the incoming operation to the property of the entity in question
        //Create an instruction set
        var prop = properties.SingleOrDefault(x => string.Equals(x.Name, op.path.TrimStart('/'), StringComparison.OrdinalIgnoreCase));

        //If there is no match then skip the property, but this would indicate that there is a problem with the patch document
        //Maybe log this as a problem?
        if (prop == null) continue;

        //Get the value from the db entity
        var incoming = prop.GetValue(entity);
        var existing = prop.GetValue(db);

        //If the incoming value is no different the db entity then skip making a change here
        if (object.Equals(incoming, existing)) continue;

        instructions.Add(new UpdateInstruction(prop.Name, incoming));

        if (prop.Name == nameof(TaskEntity.IsFinished))
        {
          //The user cannot set this value, it's dependent on the `IsFinished` property
          object? finishedOn = (bool)incoming == true ? DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified) : null;

          instructions.Add(new UpdateInstruction(nameof(TaskEntity.FinishedOn), finishedOn));
        }
      }

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
