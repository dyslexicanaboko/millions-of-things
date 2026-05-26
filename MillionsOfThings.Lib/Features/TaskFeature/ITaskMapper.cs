using MillionsOfThings.Lib.Features.TaskFeature.Models;

namespace MillionsOfThings.Lib.Features.TaskFeature;

public interface ITaskMapper
{
  TaskEntity ToEntity(TaskV1Model model);

  TaskV1Model? ToModel(TaskEntity entity);

  TaskEntity? ToEntity(int userId, TaskV1CreateModel? model);

  TaskV1PatchModel? ToPatchModel(TaskEntity? model);

  TaskEntity ToEntity(int userId, TaskV1PatchModel model);

  TaskEntity? ToEntity(TaskRecord? record);

  TaskRecord ToRecord(TaskEntity entity);

  List<TaskV1Model> ToModel(List<TaskEntity> entities);

  List<TaskEntity> ToEntity(List<TaskRecord> records);
}
