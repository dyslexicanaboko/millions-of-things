using MillionsOfThings.Lib.Exceptions;

namespace MillionsOfThings.Lib.Features.Category
{
  public class CategoryManager
    : ICategoryService
  {
    private readonly ICategoryRepository _repository;

    private readonly ICategoryValidation _validation;

    public CategoryManager(
      ICategoryRepository repository,
      ICategoryValidation validation)
    {
      _repository = repository;
      _validation = validation;
    }

    public async Task<CategoryEntity?> Get(int userId, int categoryId)
    {
      Validations.IsGreaterThanZero(categoryId, nameof(categoryId));

      return await _repository.Select(userId, categoryId);
    }

    public async Task<List<CategoryEntity>> GetAll(int userId)
    {
      Validations.IsGreaterThanZero(userId, nameof(userId));

      return await _repository.SelectAll(userId);
    }

    public async Task<CategoryEntity> Add(CategoryEntity? entity)
    {
      Validations.IsValid(_validation, entity, nameof(entity));

      if (await _repository.Exists(entity.UserId, entity.Name))
        throw new CategoryExistsAlreadyException(entity);

      entity.CategoryId = await _repository.Insert(entity);

      return entity;
    }

    public async Task Edit(CategoryEntity entity)
    {
      Validations.IsNotNull(entity, nameof(entity));
      Validations.IsGreaterThanZero(entity.CategoryId, nameof(entity.CategoryId));

      if (await _repository.Exists(entity.UserId, entity.Name))
        throw new CategoryExistsAlreadyException(entity);

      //Partial update not needed here because there's only one field
      //that can be updated at the moment.
      await _repository.Update(entity);
    }

    public async Task<CategoryRemovalResult> Remove(int userId, int categoryId)
    {
      Validations.IsGreaterThanZero(categoryId, nameof(categoryId));

      //A check will not be performed here to see if a category is in use by a task.
      //It is up to the user to make this check ahead of time.
      //All tasks will be disassociated from the category before deletion.
      
      var (isSuccess, count) = await _repository.Delete(userId, categoryId);

      return !isSuccess ? 
        throw NotFound.Category(categoryId) 
        : new CategoryRemovalResult(isSuccess, count);
    }
  }
}
