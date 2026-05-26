using FluentValidation.Results;
using MillionsOfThings.Lib.Features.CategoryFeature;
using NUnit.Framework;

namespace MillionsOfThings.UnitTests.ValidationTests
{
  [TestFixture]
  public class CategoryValidationTests
  {
    private CategoryValidation _validator;

    [SetUp]
    public void SetUp()
    {
      _validator = new CategoryValidation();
    }

    [Test]
    public void Validate_ValidCategory_ReturnsSuccess()
    {
      var entity = new CategoryEntity
      {
        UserId = 1,
        Name = "Test Category"
      };

      var result = _validator.Validate(entity);

      Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void Validate_InvalidUserId_ReturnsFailure()
    {
      var entity = new CategoryEntity
      {
        UserId = 0,
        Name = "Test Category"
      };

      var result = _validator.Validate(entity);

      Assert.That(result.IsValid, Is.False);
      Assert.That(result.Errors, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "UserId"));
    }

    [Test]
    public void Validate_EmptyName_ReturnsFailure()
    {
      var entity = new CategoryEntity
      {
        UserId = 1,
        Name = ""
      };

      var result = _validator.Validate(entity);

      Assert.That(result.IsValid, Is.False);
      Assert.That(result.Errors, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "Name"));
    }

    [Test]
    public void Validate_NameExceedsMaxLength_ReturnsFailure()
    {
      var longName = new string('A', 21);
      var entity = new CategoryEntity
      {
        UserId = 1,
        Name = longName
      };

      var result = _validator.Validate(entity);

      Assert.That(result.IsValid, Is.False);
      Assert.That(result.Errors, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "Name"));
    }
  }
}
