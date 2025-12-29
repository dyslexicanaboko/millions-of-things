using Microsoft.AspNetCore.JsonPatch;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Models.Client;

namespace MillionsOfThings.Lib.Services;

public interface ITaskService
{
  Task<TaskEntity?> Get(int userId, int taskId);

  Task<List<TaskEntity>> GetAll(int userId);

  Task<TaskEntity> Add(TaskEntity? entity);

  Task Edit(TaskEntity entity);

  Task EditPartial(int userId, int taskId, JsonPatchDocument<TaskV1PatchModel> patchDoc);

  Task Remove(int userId, int taskId);
}
