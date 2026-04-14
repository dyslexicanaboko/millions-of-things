using MillionsOfThings.Lib.Features.Category.Models;

namespace MillionsOfThings.Lib.Features.Category;

public class CategoryMapper
  : BaseMapper, ICategoryMapper
{
  public CategoryV1Model? ToModel(CategoryEntity? entity)
    => entity == null ? null : new CategoryV1Model(entity);

  public List<CategoryV1Model> ToModel(List<CategoryEntity> entities)
    => ToList(entities, ToModel)!;

  public CategoryEntity? ToEntity(int userId, CategoryV1CreateModel? model)
    => model == null ? null : new CategoryEntity(userId, model);

  public CategoryV1PatchModel? ToPatchModel(CategoryEntity? entity)
    => entity == null ? null : new CategoryV1PatchModel(entity);

  public CategoryEntity ToEntity(int userId, int categoryId, CategoryV1PatchModel model)
    => new(userId, categoryId, model);

  public CategoryEntity? ToEntity(CategoryRecord? record)
    => record == null ? null : new CategoryEntity(record);

  public CategoryRecord ToRecord(CategoryEntity entity)
    => new (){
      CategoryId = entity.CategoryId,
      UserId = entity.UserId,
      Name = entity.Name,
      CreatedOn = entity.CreatedOn,
      ModifiedOn = entity.ModifiedOn
    };

  public List<CategoryEntity> ToEntity(List<CategoryRecord> records)
    => ToListR(records, ToEntity)!;
}