namespace MillionsOfThings.Lib.Features.CategoryFeature
{
    public interface ICategory
    {
        int CategoryId { get; set; }

        int UserId { get; set; }

        string Name { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
