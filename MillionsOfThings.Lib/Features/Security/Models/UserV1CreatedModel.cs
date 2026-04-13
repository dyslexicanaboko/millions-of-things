using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionsOfThings.Lib.Features.Security.Models
{
  public class UserV1CreatedModel
  {
    public UserV1CreatedModel(UserEntity target)
    {
      UserId = target.UserId;

      Username = target.Username;
    }

    public int UserId { get; set; }

    public string Username { get; set; }
  }
}
