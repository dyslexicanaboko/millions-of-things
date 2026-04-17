using MillionsOfThings.Lib.Features.UserF.Models;

namespace MillionsOfThings.Lib.Features.UserF
{
  public class UserMapper
    : BaseMapper, IUserMapper
  {
    public UserV1Model? ToModel(UserEntity? entity)
    {
      if (entity == null) return null;

      var model = new UserV1Model(entity);

      return model;
    }

    public UserEntity? ToEntity(UserV1CreateModel? target)
    {
      if (target == null) return null;

      var model = new UserEntity(target);

      return model;
    }

    public UserEntity? ToEntity(UserRecord? record)
      => record == null ? null : new UserEntity(record);

    public UserV1CreatedModel? ToCreatedModel(UserEntity? target)
    {
      if (target == null) return null;

      var model = new UserV1CreatedModel(target);

      return model;
    }

    public UserRecord ToRecord(UserEntity entity)
      => new UserRecord
      {
        UserId = entity.UserId,
        IsAllowed = entity.IsAllowed,
        Username = entity.Username,
        Password = entity.Password,
        CreatedOn = entity.CreatedOn
      };

    public List<UserV1Model> ToModel(List<UserEntity> target) => ToList(target, ToModel)!;
    
    public List<UserEntity> ToList(List<UserRecord> record) => ToListR(record, ToEntity)!;
  }
}
