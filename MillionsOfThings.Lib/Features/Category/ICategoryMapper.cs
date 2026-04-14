using MillionsOfThings.Lib.Features.Category.Models;

namespace MillionsOfThings.Lib.Features.Category;

public interface ICategoryMapper
{
  CategoryV1Model? ToModel(CategoryEntity? entity);

  List<CategoryV1Model> ToModel(List<CategoryEntity> entities);

  CategoryEntity? ToEntity(int userId, CategoryV1CreateModel? model);

  CategoryEntity ToEntity(int userId, int categoryId, CategoryV1PatchModel model);

  CategoryV1PatchModel? ToPatchModel(CategoryEntity? entity);

  CategoryEntity? ToEntity(CategoryRecord? record);

  CategoryRecord ToRecord(CategoryEntity entity);

  List<CategoryEntity> ToEntity(List<CategoryRecord> records);
}
