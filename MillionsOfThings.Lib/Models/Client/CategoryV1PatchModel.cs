using MillionsOfThings.Lib.Entities;
using System;

namespace MillionsOfThings
{
    public class CategoryV1PatchModel : ICategory
    {
        public CategoryV1PatchModel()
        {

        }

        public CategoryV1PatchModel(ICategory target)
        {
            CategoryId = target.CategoryId;
            UserId = target.UserId;
            Name = target.Name;
            CreatedOn = target.CreatedOn;
            ModifiedOn = target.ModifiedOn;
        }

        public int CategoryId { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
