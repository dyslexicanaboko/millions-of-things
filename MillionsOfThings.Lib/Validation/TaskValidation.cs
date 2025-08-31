using FluentValidation;
using MillionsOfThings.Lib.DataAccess;
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
    public TaskValidation(ICategoryRepository repository)
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
            .Must((entity, categoryId) =>
            {
              if (!categoryId.HasValue) return true;

              var t = repository
                .Using(x => x.Exists(entity.UserId, categoryId.Value));

              t.Wait();
              
              return t.Result;
            })
            .WithMessageAndErrorCode(MappingNotFound(nameof(TaskEntity.CategoryId)));
        });

      //RuleFor(r => r.TaskId)
      //  .GreaterThan(0)
      //  .WithMessageAndErrorCode(NotGreaterThanZero(nameof(TaskEntity.TaskId)));
    }
  }
}
