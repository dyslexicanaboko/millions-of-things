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

    public async Task<CategoryEntity?> Get(int userId, int categoryId)
    {
      Validations.IsGreaterThanZero(categoryId, nameof(categoryId));

      var dbEntity = await _repository.Using(x => x.Select(userId, categoryId));

      return dbEntity;
    }

    public async Task<List<CategoryEntity>> GetAll(int userId)
    {
      Validations.IsGreaterThanZero(userId, nameof(userId));

      var lst = (await _repository.Using(x => x.SelectAll(userId))).ToList();

      return lst;
    }

    public async Task<CategoryEntity> Add(CategoryEntity? entity)
    {
      Validations.IsValid(_validation, entity, nameof(entity));

      if (await _repository.Using(x => x.Exists(entity.UserId, entity.Name)))
        throw new CategoryExistsAlreadyException(entity);

      entity.CategoryId = await _repository.Using(x => x.Insert(entity));

      return entity;
    }

    public async Task Edit(CategoryEntity entity)
    {
      Validations.IsNotNull(entity, nameof(entity));
      Validations.IsGreaterThanZero(entity.CategoryId, nameof(entity.CategoryId));

      if (await _repository.Using(x => x.Exists(entity.UserId, entity.Name)))
        throw new CategoryExistsAlreadyException(entity);

      //Partial update not needed here because there's only one field
      //that can be updated at the moment.
      await _repository.Using(x => x.Update(entity));
    }

    public async Task Remove(int userId, int categoryId)
    {
      Validations.IsGreaterThanZero(categoryId, nameof(categoryId));

      await _repository.Using(x => x.Delete(userId, categoryId));
    }
  }
}
