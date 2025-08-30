using MillionsOfThings.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionsOfThings.Lib.Exceptions
{
  public class CategoryExistsAlreadyException
    : EntityExistsAlreadyException
  {
    public CategoryExistsAlreadyException(CategoryEntity category)
      : base(GetMessage(category))
    {
    }

    public override int ErrorCode { get; set; } = ErrorCodes.Errors.CategoryExistsAlready;

    private static string GetMessage(CategoryEntity category)
      => $"A category with name `{category.Name}` already exists for this user.";
  }
}
