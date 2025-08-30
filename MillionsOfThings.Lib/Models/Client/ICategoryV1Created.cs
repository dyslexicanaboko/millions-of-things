using MillionsOfThings.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionsOfThings.Lib.Models.Client
{
  public class CategoryV1CreatedModel
  {
    public CategoryV1CreatedModel()
    {

    }

    public CategoryV1CreatedModel(CategoryEntity entity)
    {
      CategoryId = entity.CategoryId;
      UserId = entity.UserId;
      Name = entity.Name;
    }

    int CategoryId { get; set; }
    int UserId { get; set; }
    string Name { get; set; }
  }
}
