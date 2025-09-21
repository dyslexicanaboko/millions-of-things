using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionsOfThings.Lib
{
  public static class StandardValues
  {
    public static DateTime GetUtcNow() 
      => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
  }
}
