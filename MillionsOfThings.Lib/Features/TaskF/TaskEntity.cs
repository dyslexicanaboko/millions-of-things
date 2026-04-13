using MillionsOfThings.Lib.Features.TaskF.Models;

namespace MillionsOfThings.Lib.Features.TaskF
{
  public class TaskEntity
      : ITask, IEquatable<TaskEntity>
  {
    public TaskEntity()
    {

    }

    public TaskEntity(int userId, TaskV1CreateModel model)
    {
      UserId = userId;
      CategoryId = model.CategoryId;
      Description = model.Description;
    }

    public TaskEntity(int userId, TaskV1PatchModel model)
    {
      UserId = userId;
      CategoryId = model.CategoryId;
      Description = model.Description;
      IsFinished = model.IsFinished;
    }

    public TaskEntity(TaskV1Model model)
    {
      TaskId = model.TaskId;
      UserId = model.UserId;
      CategoryId = model.CategoryId;
      Description = model.Description;
      IsFinished = model.IsFinished;
      FinishedOn = model.FinishedOn;
      CreatedOn = model.CreatedOn;
      ModifiedOn = model.ModifiedOn;
    }

    //TODO: Should there be an interface between Entity and Record?
    public TaskEntity(TaskRecord record)
    {
      TaskId = record.TaskId;
      UserId = record.UserId;
      CategoryId = record.CategoryId;
      Description = record.Description;
      IsFinished = record.IsFinished;
      FinishedOn = record.FinishedOn;
      CreatedOn = record.CreatedOn;
      ModifiedOn = record.ModifiedOn;
    }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public int? CategoryId { get; set; }

    public string Description { get; set; }

    public bool IsFinished { get; set; }

    public DateTime? FinishedOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public override bool Equals(object? obj) => this.Equals(obj as TaskEntity);

    public bool Equals(TaskEntity? other)
    {
      if (other is null) return false;

      if (ReferenceEquals(this, other)) return true;

      if (GetType() != other.GetType()) return false;

      return
        TaskId == other.TaskId &&
        UserId == other.UserId &&
        CategoryId == other.CategoryId &&
        Description == other.Description &&
        IsFinished == other.IsFinished &&
        FinishedOn == other.FinishedOn &&
        CreatedOn == other.CreatedOn &&
        ModifiedOn == other.ModifiedOn;
    }

    public override int GetHashCode() =>
      TaskId.GetHashCode() +
      UserId.GetHashCode() +
      CategoryId.GetHashCode() +
      Description.GetHashCode() +
      IsFinished.GetHashCode() +
      FinishedOn.GetHashCode() +
      CreatedOn.GetHashCode() +
      ModifiedOn.GetHashCode();

    public static bool operator ==(TaskEntity? lhs, TaskEntity? rhs)
    {
      if (lhs is not null) return lhs.Equals(rhs);

      return rhs is null;
    }

    public static bool operator !=(TaskEntity? lhs, TaskEntity? rhs) => !(lhs == rhs);
  }
}
