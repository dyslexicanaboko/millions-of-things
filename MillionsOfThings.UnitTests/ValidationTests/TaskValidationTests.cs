using FakeItEasy;
using FluentValidation.Results;
using MillionsOfThings.Lib.Features.CategoryFeature;
using MillionsOfThings.Lib.Features.TaskFeature;
using NUnit.Framework;

namespace MillionsOfThings.UnitTests.ValidationTests
{
  [TestFixture]
  public class TaskValidationTests
  {
    private ICategoryRepository _categoryRepository;
    private TaskValidation _validator;

    [SetUp]
    public void SetUp()
    {
      _categoryRepository = A.Fake<ICategoryRepository>();
      _validator = new TaskValidation(_categoryRepository);
    }

    [Test]
    public void Validate_ValidTask_ReturnsSuccess()
    {
      var entity = new TaskEntity
      {
        UserId = 1,
        CategoryId = 2,
        Description = "Test"
      };

      A.CallTo(() => _categoryRepository.Exists(1, 2)).Returns(true);

      var result = _validator.Validate(entity);

      Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void Validate_InvalidUserId_ReturnsFailure()
    {
      var entity = new TaskEntity
      {
        UserId = 0,
        CategoryId = 2,
        Description = "Test"
      };

      var result = _validator.Validate(entity);

      Assert.That(result.IsValid, Is.False);
      Assert.That(result.Errors, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "UserId"));
    }

    [Test]
    public void Validate_CategoryIdNotGreaterThanZero_ReturnsFailure()
    {
      var entity = new TaskEntity
      {
        UserId = 1,
        CategoryId = 0,
        Description = "Test"
      };

      var result = _validator.Validate(entity);

      Assert.That(result.IsValid, Is.False);
      Assert.That(result.Errors, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "CategoryId"));
    }

    [Test]
    public void Validate_CategoryIdDoesNotExist_ReturnsFailure()
    {
      var entity = new TaskEntity
      {
        UserId = 1,
        CategoryId = 5,
        Description = "Test"
      };

      A.CallTo(() => _categoryRepository.Exists(1, 5)).Returns(false);

      var result = _validator.Validate(entity);

      Assert.That(result.IsValid, Is.False);
      Assert.That(result.Errors, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "CategoryId"));
    }

    [Test]
    public void Validate_EmptyDescription_ReturnsFailure()
    {
      var entity = new TaskEntity
      {
        UserId = 1,
        CategoryId = 2,
        Description = ""
      };

      var result = _validator.Validate(entity);

      Assert.That(result.IsValid, Is.False);
      Assert.That(result.Errors, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "Description"));
    }

    [Test]
    public void Validate_NullCategoryId_IsValid()
    {
      var entity = new TaskEntity
      {
        UserId = 1,
        CategoryId = null,
        Description = "Test"
      };

      var result = _validator.Validate(entity);

      Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void Validate_DescriptionTooLong_ReturnsFailure()
    {
      var entity = new TaskEntity
      {
        UserId = 1,
        CategoryId = 2,
        Description = new string('a', 256) // 256 characters, exceeding the limit
      };
      var result = _validator.Validate(entity);
      Assert.That(result.IsValid, Is.False);
      Assert.That(result.Errors, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "Description"));
    }
  }
}
