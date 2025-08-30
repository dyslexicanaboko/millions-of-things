using FluentValidation;
using MillionsOfThings.Lib.Entities;
using static MillionsOfThings.Lib.Exceptions.InvalidArgument;

namespace MillionsOfThings.Lib.Validation
{
  public interface ITaskValidation
    : IFluentValidation<TaskEntity>
  {
  }

  public class TaskValidation
    : AbstractValidator<TaskEntity>, ITaskValidation
  {
    public TaskValidation()
    {
      RuleFor(r => r.UserId)
        .GreaterThan(0)
        .WithMessageAndErrorCode(User);

      RuleFor(r => r.CategoryId)
        .Must((_, categoryId) =>
        {
          //Null is acceptable, just means this is not categorized
          if (!categoryId.HasValue) return true;

          //If a value is provided it must be greater than zero
          if (categoryId.Value <= 0) return false;

          return true;
        })
        .WithMessageAndErrorCode(NotGreaterThanZero(nameof(TaskEntity.CategoryId)))
        .DependentRules(() =>
        {
          //This will require caching at one point
          RuleFor(r => r.CategoryId)
            .Must((_, categoryId) =>
            {
              if (!categoryId.HasValue) return true;

              //CategoryId is not null and has a value greater than zero
              //Verify that it maps to something in the database
              return true;
            })
            .WithMessageAndErrorCode(MappingNotFound(nameof(TaskEntity.CategoryId)));
        });

      //RuleFor(r => r.TaskId)
      //  .GreaterThan(0)
      //  .WithMessageAndErrorCode(NotGreaterThanZero(nameof(TaskEntity.TaskId)));
    }
  }
}
