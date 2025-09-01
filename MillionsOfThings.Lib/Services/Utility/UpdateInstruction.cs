using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillionsOfThings.Lib.Services.Utility
{
  public class UpdateInstruction
  {
    public UpdateInstruction(string property, object? value)
    {
      Property = property;
      Value = value;
    }

    public string Property { get; set; }
    public object? Value { get; set; }
  }
}
