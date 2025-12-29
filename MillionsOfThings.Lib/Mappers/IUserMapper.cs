using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Models;
using MillionsOfThings.Lib.Models.Client;

namespace MillionsOfThings.Lib.Mappers
{
  public interface IUserMapper
  {
    UserV1CreatedModel? ToCreatedModel(UserEntity? target);
    UserEntity? ToEntity(UserV1CreateModel? target);
    List<UserV1Model> ToModel(List<UserEntity> target);
    UserV1Model? ToModel(UserEntity? entity);
  }
}