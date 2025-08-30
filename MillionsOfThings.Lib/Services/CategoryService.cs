using MillionsOfThings.Lib.DataAccess;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Exceptions;
using MillionsOfThings.Lib.Validation;

namespace MillionsOfThings.Lib.Services
{
  public class CategoryService
    : ICategoryService
  {
    private readonly ICategoryRepository _repository;

    private readonly ICategoryValidation _validation;

    public CategoryService(
      ICategoryRepository repository,
      ICategoryValidation validation)
    {
      _repository = repository;
      _validation = validation;
    }

    public async Task<CategoryEntity?> GetCategory(int categoryId)
    {
      Validations.IsGreaterThanZero(categoryId, nameof(categoryId));

      var dbEntity = await _repository.Using(x => x.Select(categoryId));

      return dbEntity;
    }

    public async Task<CategoryEntity> Add(CategoryEntity? entity)
    {
      Validations.IsValid(_validation, entity, nameof(entity));

      if (await _repository.Using(x => x.Exists(entity)))
        throw new CategoryExistsAlreadyException(entity);

      entity.CategoryId = await _repository.Using(x => x.Insert(entity));

      return entity;
    }

    public async Task Edit(CategoryEntity entity)
    {
      Validations.IsNotNull(entity, nameof(entity));

      if (await _repository.Using(x => x.Exists(entity)))
        throw new CategoryExistsAlreadyException(entity);

      await _repository.Using(x => x.Update(entity));
    }

    public async Task Remove(int categoryId)
    {
      Validations.IsGreaterThanZero(categoryId, nameof(categoryId));

      await _repository.Using(x => x.Delete(categoryId));
    }
  }
}
