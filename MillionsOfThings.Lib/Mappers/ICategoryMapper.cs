using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Models;
using MillionsOfThings.Lib.Models.Client;

namespace MillionsOfThings.Lib.Mappers;

public interface ICategoryMapper
{
  CategoryV1Model? ToModel(CategoryEntity? entity);
  
  CategoryEntity? ToEntity(int userId, CategoryV1CreateModel? model);

  CategoryEntity ToEntity(CategoryV1PatchModel model);

  CategoryV1PatchModel? ToPatchModel(CategoryEntity? entity);
}
