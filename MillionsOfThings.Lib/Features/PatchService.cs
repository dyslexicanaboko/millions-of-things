using Microsoft.AspNetCore.JsonPatch;
using MillionsOfThings.Lib.Utility;

namespace MillionsOfThings.Lib.Features;

public class PatchService
{
  // Goal is to extract the Patch Document as a list of properties that are allowed to be updated.
  // Any unsupported properties are ignored.
  public List<UpdateInstruction> GetInstructions(JsonPatchDocument patchDocument, List<string> allowedProperties)
  {
    var instructions = new List<UpdateInstruction>(patchDocument.Operations.Count);

    foreach (var op in patchDocument.Operations)
    {
      //For each item in the operations list (which is what was explicitly provided)
      //Match the incoming operation to the property of the entity in question
      //Create an instruction set
      var prop = allowedProperties.SingleOrDefault(x
        => string.Equals(x, op.path.TrimStart('/'), StringComparison.OrdinalIgnoreCase));

      //If there is no match then skip the property, but this would indicate that there is a problem with the patch document
      //Maybe log this as a problem?
      if (prop == null) continue;

      instructions.Add(new UpdateInstruction(prop, op.value));
    }

    return instructions;
  }
}