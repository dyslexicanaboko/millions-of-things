using FakeItEasy;
using FluentValidation.Results;
using MillionsOfThings.Lib.Exceptions;
using MillionsOfThings.Lib.Features.Category;
using NUnit.Framework;

namespace MillionsOfThings.UnitTests.ServicesTests
{
  [TestFixture]
  public class CategoryServiceTests
  {
    private ICategoryRepository _repository;
    private ICategoryValidation _validation;
    private CategoryManager _service;

    [SetUp]
    public void SetUp()
    {
      _repository = A.Fake<ICategoryRepository>();
      _validation = A.Fake<ICategoryValidation>();
      _service = new CategoryManager(_repository, _validation);
    }

    [Test]
    public async Task Validate_AttemptsToCreateExistingCategory_ThrowsExceptionOnSecondAttempt()
    {
      // Arrange
      var userId = 1;
      var categoryName = "Category A";
      var category = new CategoryEntity
      {
        UserId = userId,
        Name = categoryName,
        CreatedOn = DateTime.UtcNow
      };

      // First call: category does not exist
      A.CallTo(() => _repository.Exists(userId, categoryName)).Returns(false);
      A.CallTo(() => _validation.Validate(category)).Returns(new ValidationResult());
      A.CallTo(() => _repository.Insert(category)).Returns(1);

      // Act
      var result = await _service.Add(category);

      // Assert
      Assert.That(result, Is.Not.Null);
      A.CallTo(() => _repository.Insert(category)).MustHaveHappenedOnceExactly();

      // Second call: category already exists
      A.CallTo(() => _repository.Exists(userId, categoryName)).Returns(true);

      // Act & Assert
      Assert.ThrowsAsync<CategoryExistsAlreadyException>(async () => await _service.Add(category));
    }
  }
}
