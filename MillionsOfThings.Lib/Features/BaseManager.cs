using Microsoft.AspNetCore.JsonPatch;
using MillionsOfThings.Lib.Utility;
using System.Reflection;

namespace MillionsOfThings.Lib.Features;

public abstract class BaseManager
{
  // Version 1
  /// <summary>
  /// Version one of extracting differences from a patch document. This is based on the
  /// JsonPatchDocument and is more tightly coupled to the ASP.NET Core framework. It
  /// uses reflection to compare the incoming values from the patch document with the
  /// existing values in the database entity and creates a list of update instructions
  /// for any properties that have differences. The whenDirty action allows for additional
  /// processing when a difference is found, such as logging or validation.
  /// </summary>
  /// <typeparam name="TEntity"></typeparam>
  /// <typeparam name="TPatchModel"></typeparam>
  /// <param name="db"></param>
  /// <param name="entity"></param>
  /// <param name="patchDoc"></param>
  /// <param name="whenDirty"></param>
  /// <returns></returns>
  protected static List<UpdateInstruction> GetDifferences<TEntity, TPatchModel>(
    TEntity db, 
    TEntity entity, 
    JsonPatchDocument<TPatchModel> patchDoc,
    Action<List<UpdateInstruction>, PropertyInfo> whenDirty)
    where TPatchModel : class, new()
  {
    var properties = entity.GetType().GetProperties();
    var instructions = new List<UpdateInstruction>(patchDoc.Operations.Count);

    foreach (var op in patchDoc.Operations)
    {
      //For each item in the operations list (which is what was explicitly provided)
      //Match the incoming operation to the property of the entity in question
      //Create an instruction set
      var prop = properties.SingleOrDefault(x 
        => string.Equals(x.Name, op.path.TrimStart('/'), StringComparison.OrdinalIgnoreCase));

      //If there is no match then skip the property, but this would indicate that there is a problem with the patch document
      //Maybe log this as a problem?
      if (prop == null) continue;

      //Get the value from the db entity
      var incoming = prop.GetValue(entity);
      var existing = prop.GetValue(db);

      //If the incoming value has no differences from the db entity, then skip making a change here
      if (Equals(incoming, existing)) continue;

      instructions.Add(new UpdateInstruction(prop.Name, incoming));

      whenDirty(instructions, prop);
    }

    return instructions;
  }

  // Version 2
  /// <summary>
  /// Version two of extracting differences from a patch document. This version is more generic and
  /// is not tied to the ASP.NET Core framework. It takes a list of update instructions, which can
  /// be created from any source (not just a JsonPatchDocument), and compares the incoming values
  /// with the existing values in the database entity. It uses reflection to access the properties
  /// of the entity and creates a list of update instructions for any properties that have differences.
  /// The whenDirty action allows for additional processing when a difference is found, such as logging
  /// or validation.
  /// </summary>
  /// <typeparam name="TEntity"></typeparam>
  /// <param name="db"></param>
  /// <param name="patchInstructions"></param>
  /// <param name="whenDirty"></param>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  protected static List<UpdateInstruction> GetDifferences<TEntity>(
    TEntity db,
    List<UpdateInstruction> patchInstructions,
    Action<List<UpdateInstruction>, PropertyInfo>? whenDirty = null)
    where TEntity : class, new()
  {
    var properties = typeof(TEntity).GetProperties();
    var updateInstructions = new List<UpdateInstruction>(patchInstructions.Count);

    foreach (var op in patchInstructions)
    {
      //For each item in the operations list (which is what was explicitly provided)
      //Match the incoming operation to the property of the entity in question
      //Create an instruction set
      var prop = properties.SingleOrDefault(x
        => string.Equals(x.Name, op.Property, StringComparison.OrdinalIgnoreCase));

      //This cannot happen because the patch instructions should have been validated before this point,
      //but if there is no match then skip the property, but this would indicate that there is a problem
      //with the patch instructions
      if (prop == null) throw new InvalidOperationException($"The property '{op.Property}' does not exist on type '{typeof(TEntity).Name}'.");

      //Get the value from the db entity
      var incoming = prop.GetValue(op.Value);
      var existing = prop.GetValue(db);

      //If the incoming value has no differences from the db entity, then skip making a change here
      if (Equals(incoming, existing)) continue;

      updateInstructions.Add(new UpdateInstruction(prop.Name, incoming));

      whenDirty?.Invoke(updateInstructions, prop);
    }

    return updateInstructions;
  }
}