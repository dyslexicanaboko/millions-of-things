using MillionsOfThings.Lib.DataAccess;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Exceptions;
using MillionsOfThings.Lib.Results;
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

    public async Task<CategoryRemovalResult> Remove(int userId, int categoryId)
    {
      Validations.IsGreaterThanZero(categoryId, nameof(categoryId));

      //A check will not be performed here to see if a category is in use by a task.
      //It is up to the user to make this check ahead of time.
      //All tasks will be disassociated from the category before deletion.

      await _repository.BeginTransaction();

      var count = await _repository.DetachFromTasks(userId, categoryId);

      var isSuccess = await _repository.Delete(userId, categoryId) > 0;

      if (!isSuccess)
      {
        await _repository.RollbackTransaction();

        throw NotFound.Category(categoryId);
      }

      await _repository.CommitTransaction();

      return new CategoryRemovalResult(isSuccess, count);
    }
  }
}
