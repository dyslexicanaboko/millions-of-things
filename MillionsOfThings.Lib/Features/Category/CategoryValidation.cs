using FluentValidation;
using static MillionsOfThings.Lib.Exceptions.InvalidArgument;

namespace MillionsOfThings.Lib.Features.Category
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

      RuleFor(r => r.Name)
        .TestStringLength(nameof(CategoryEntity.Name), 1, 20);
    }
  }
}
