using System.Data;

namespace MillionsOfThings.Lib.Utility;

/// <summary>
/// Well known schema definition for columns of a table.
/// </summary>
/// <param name="property">Property name in the entity.</param>
/// <param name="name">Column name in the database.</param>
/// <param name="dbType">Database type of the column.</param>
/// <param name="size">Size of the column (if applicable).</param>
/// <param name="scale">Scale of the column (if applicable).</param>
public class ColumnSchema(
  string property,
  string name,
  DbType dbType,
  int? size = null,
  byte? scale = null)
{
  public string Property { get; set; } = property;

  public string Name { get; set; } = name;

  public DbType DbType { get; set; } = dbType;

  public int? Size { get; set; } = size;

  public byte? Scale { get; set; } = scale;
}
