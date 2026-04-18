namespace MillionsOfThings.Lib.Utility;

/// <summary>
/// Represents an instruction to update a specific property with a new value.
/// </summary>
/// <param name="property">The name of the property to update.</param>
/// <param name="value">The new value to assign to the property.</param>
public class UpdateInstruction(string property, object? value)
{ 
  public string Property { get; set; } = property;
  
  public object? Value { get; set; } = value;
}
