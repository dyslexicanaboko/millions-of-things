using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Models;
using MillionsOfThings.Lib.Models.Client;

namespace MillionsOfThings.Lib.Mappers
{
  public class CategoryMapper
    : BaseMapper, ICategoryMapper
  {
    public CategoryV1Model? ToModel(CategoryEntity? entity)
      => entity == null ? null : new CategoryV1Model(entity);

    public CategoryEntity? ToEntity(int userId, CategoryV1CreateModel? model)
      => model == null ? null : new CategoryEntity(userId, model);

    public CategoryV1PatchModel? ToPatchModel(CategoryEntity? entity)
      => entity == null ? null : new CategoryV1PatchModel(entity);

    public CategoryEntity ToEntity(CategoryV1PatchModel model)
      => new(model);
  }
}
