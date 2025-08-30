using FluentValidation;
using MillionsOfThings.Lib.Entities;
using static MillionsOfThings.Lib.Exceptions.InvalidArgument;

namespace MillionsOfThings.Lib.Validation
{
  public interface ICategoryValidation
    : IFluentValidation<CategoryEntity>
  {
  }

  public class CategoryValidation
    : AbstractValidator<CategoryEntity>, ICategoryValidation
  {
    public CategoryValidation()
    {
      RuleFor(r => r.UserId)
        .GreaterThan(0)
        .WithMessageAndErrorCode(User);

      RuleFor(r => r.Name)
        .NotEmpty()
        .WithMessageAndErrorCode(Empty(nameof(CategoryEntity.Name)));
    }
  }
}
