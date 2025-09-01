using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionsOfThings.Lib.DataAccess.Utility
{
  public class ColumnSchema
  {
    public ColumnSchema(string property, string name, DbType dbType, int? size = null, byte? scale = null)
    {
      Property = property;
      Name = name;
      DbType = dbType;
      Size = size;
      Scale = scale;
    }

    public string Property { get; set; }
    public string Name { get; set; }
    public DbType DbType { get; set; }
    public int? Size { get; set; }
    public byte? Scale { get; set; }
  }
}
