using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Models;
using MillionsOfThings.Lib.Models.Client;
using MillionsOfThings.Lib.Records;

namespace MillionsOfThings.Lib.Mappers
{
  public class TaskMapper
    : BaseMapper, ITaskMapper
  {
    public TaskEntity ToEntity(TaskV1Model model)
      => new (model);

    public TaskEntity? ToEntity(int userId, TaskV1CreateModel? model)
      => model == null ? null : new TaskEntity(userId, model);

    public TaskV1PatchModel? ToPatchModel(TaskEntity? model)
      => model == null ? null : new TaskV1PatchModel(model);

    public TaskEntity ToEntity(int userId, TaskV1PatchModel model)
      => new (userId, model);

    public TaskV1Model? ToModel(TaskEntity? entity)
      => entity == null ? null : new TaskV1Model(entity);

    public IList<TaskV1Model> ToModel(IList<TaskEntity> entities)
      => ToList(entities, ToModel);

    public TaskEntity? ToEntity(TaskRecord? record)
      => record == null ? null : new TaskEntity(record);

    public TaskRecord ToRecord(TaskEntity entity)
      => new()
      {
        TaskId = entity.TaskId,
        UserId = entity.UserId,
        CategoryId = entity.CategoryId,
        Description = entity.Description,
        IsFinished = entity.IsFinished,
        FinishedOn = entity.FinishedOn,
        CreatedOn = entity.CreatedOn,
        ModifiedOn = entity.ModifiedOn,
      };

    public IList<TaskEntity> ToEntity(IList<TaskRecord> records)
      => ToListR(records, ToEntity);
  }
}
