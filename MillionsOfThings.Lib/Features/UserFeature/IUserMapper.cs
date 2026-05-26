using MillionsOfThings.Lib.Features.UserFeature.Models;

namespace MillionsOfThings.Lib.Features.UserFeature
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