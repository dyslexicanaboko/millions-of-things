using MillionsOfThings.Lib.Features.CategoryFeature.Models;

namespace MillionsOfThings.Lib.Features.CategoryFeature
{
  public class CategoryEntity
    : ICategory, IEquatable<CategoryEntity>, IComparable
  {
    public CategoryEntity()
    {
    }

    public CategoryEntity(ICategory target)
    {
      CategoryId = target.CategoryId;
      UserId = target.UserId;
      Name = target.Name;
      CreatedOn = target.CreatedOn;
      ModifiedOn = target.ModifiedOn;
    }

    public CategoryEntity(CategoryRecord record)
    {
      CategoryId = record.CategoryId;
      UserId = record.UserId;
      Name = record.Name;
      CreatedOn = record.CreatedOn;
      ModifiedOn = record.ModifiedOn;
    }

    public CategoryEntity(int userId, CategoryV1CreateModel model)
    {
      UserId = userId;
      Name = model.Name;
    }

    public CategoryEntity(int userId, int categoryId, CategoryV1PatchModel model)
    {
      UserId = userId;
      CategoryId = categoryId;
      Name = model.Name;
    }

    public int CategoryId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int CompareTo(object obj)
    {
      //This is starter code - it is meant to be changed appropriately
      var n = (CategoryEntity)obj;

      if (CategoryId == n.CategoryId) return 0;

      if (CategoryId > n.CategoryId) return 1;

      //if (CategoryId < n.CategoryId)
      return -1;
    }

    public bool Equals(CategoryEntity? other)
    {
      if (other is null) return false;

      if (ReferenceEquals(this, other)) return true;

      if (GetType() != other.GetType()) return false;

      return
        CategoryId == other.CategoryId &&
        UserId == other.UserId &&
        Name == other.Name &&
        CreatedOn == other.CreatedOn &&
        ModifiedOn == other.ModifiedOn;
    }

    public override bool Equals(object? obj) => Equals(obj as CategoryEntity);

    public override int GetHashCode() =>
      CategoryId.GetHashCode() +
      UserId.GetHashCode() +
      Name.GetHashCode() +
      CreatedOn.GetHashCode() +
      ModifiedOn.GetHashCode();

    public static bool operator ==(CategoryEntity? lhs, CategoryEntity? rhs)
    {
      if (lhs is not null) return lhs.Equals(rhs);

      return rhs is null;
    }

    public static bool operator !=(CategoryEntity? lhs, CategoryEntity? rhs) => !(lhs == rhs);
  }
}
