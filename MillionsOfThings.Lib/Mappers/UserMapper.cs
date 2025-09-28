using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Models;
using MillionsOfThings.Lib.Models.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionsOfThings.Lib.Mappers
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

    public UserV1CreatedModel? ToCreatedModel(UserEntity? target)
    {
      if (target == null) return null;

      var model = new UserV1CreatedModel(target);

      return model;
    }

    public IList<UserV1Model> ToModel(IList<UserEntity> target) => ToList(target, ToModel);
  }
}
