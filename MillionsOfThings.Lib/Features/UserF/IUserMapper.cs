using MillionsOfThings.Lib.Features.UserF.Models;

namespace MillionsOfThings.Lib.Features.UserF
{
  public interface IUserMapper
  {
    UserV1CreatedModel? ToCreatedModel(UserEntity? target);
    UserEntity? ToEntity(UserV1CreateModel? target);
    List<UserV1Model> ToModel(List<UserEntity> target);
    UserV1Model? ToModel(UserEntity? entity);

    UserEntity? ToEntity(UserRecord? record);

    UserRecord ToRecord(UserEntity entity);

    List<UserEntity> ToList(List<UserRecord> record);
  }
}