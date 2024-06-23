using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Models;

namespace MillionsOfThings.Lib.Mappers;

public interface ITaskMapper
{
  TaskEntity ToEntity(TaskModel model);

  TaskEntity ToEntity(ITask target);

  TaskModel ToModel(TaskEntity entity);

  TaskModel ToModel(ITask target);
}
