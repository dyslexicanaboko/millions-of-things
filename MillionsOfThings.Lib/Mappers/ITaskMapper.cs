using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Models;
using MillionsOfThings.Lib.Models.Client;

namespace MillionsOfThings.Lib.Mappers;

public interface ITaskMapper
{
  TaskEntity ToEntity(TaskModel model);

  TaskEntity ToEntity(ITask target);

  TaskModel ToModel(TaskEntity entity);

  TaskModel ToModel(ITask target);

  TaskEntity? ToEntity(int userId, TaskV1CreateModel? model);

  TaskV1PatchModel? ToPatchModel(TaskEntity? model);

  TaskEntity ToEntity(int userId, TaskV1PatchModel model);
}
