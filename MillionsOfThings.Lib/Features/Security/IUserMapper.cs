using MillionsOfThings.Lib.Features.Security.Models;

namespace MillionsOfThings.Lib.Features.Security
{
  public interface IUserMapper
  {
    UserV1CreatedModel? ToCreatedModel(UserEntity? target);
    UserEntity? ToEntity(UserV1CreateModel? target);
    List<UserV1Model> ToModel(List<UserEntity> target);
    UserV1Model? ToModel(UserEntity? entity);
  }
}