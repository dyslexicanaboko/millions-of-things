using MillionsOfThings.Lib.Features.UserFeature.Models;

namespace MillionsOfThings.Lib.Features.UserFeature
{
  public interface IUserMapper
  {
    List<UserV1Model> ToModel(List<UserEntity> target);

    UserV1Model? ToModel(UserEntity? entity);

    UserEntity? ToEntity(UserRecord? record);

    UserRecord ToRecord(UserEntity entity);

    List<UserEntity> ToList(List<UserRecord> record);
  }
}