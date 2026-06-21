using Dapper;
using MillionsOfThings.Lib.Utility;
using Npgsql;
using System.Data;

namespace MillionsOfThings.Lib.Features;

public abstract class BaseRepository
{
  protected string? ConnectionString;

  protected BaseRepository()
  {
    //NOTE: Do not instantiate objects here. This is here for the purposes of DI only.
  }

  //TODO: Refactor to introduce a provider for getting the connection
  //Primary constructor
  protected BaseRepository(IAppConfiguration configuration) => ConnectionString = configuration.GetConnectionString();

  protected async Task<NpgsqlConnection> GetConnection(CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(ConnectionString)) throw new ApplicationException("Connection string cannot be blank, whitespace, or null.");

    var conn = new NpgsqlConnection(ConnectionString);
      
    await conn.OpenAsync(cancellationToken);

    return conn;
  }

  protected static SqlMapper.ICustomQueryParameter GetTvpIntegerList(IList<int> integerList)
  {
    var dt = new DataTable("IntegerList");
    dt.Columns.Add("IntValue", typeof(int));

    foreach (var i in integerList)
    {
      var dr = dt.NewRow();
      dr["IntValue"] = i;

      dt.Rows.Add(dr);
    }

    return dt.AsTableValuedParameter("dbo.IntegerList");
  }

  /// <summary>
  /// Automatically construct a partial update statement based on the provided instructions.
  /// This is useful for PATCH endpoints where you want to allow partial updates.
  /// </summary>
  /// <param name="updateTemplate"></param>
  /// <param name="updatePartialColumns"></param>
  /// <param name="p"></param>
  /// <param name="instructions"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentException"></exception>
  protected async Task UpdatePartial(
    string updateTemplate,
    List<ColumnSchema> updatePartialColumns,
    DynamicParameters p, 
    List<UpdateInstruction> instructions)
  {
    if (instructions.Count == 0) return;

    var lst = new List<string>(instructions.Count);

    foreach (var instr in instructions)
    {
      var col = updatePartialColumns.SingleOrDefault(x => x.Property == instr.Property);

      if (col == null) throw new ArgumentException($"The property '{instr.Property}' is not valid for partial updates.", nameof(instructions));

      lst.Add($"{col.Name} = @{col.Name}");

      p.Add(name: col.Name, dbType: col.DbType, value: instr.Value, size: col.Size, scale: col.Scale);
    }

    await using var connection = await GetConnection();

    var sql = string.Format(updateTemplate, string.Join(", ", lst));

    await connection.ExecuteAsync(sql, p);
  }

  protected static DynamicParameters AddUserIdParameter(DynamicParameters p, int userId)
  {
    p.Add("@user_id", dbType: DbType.Int32, value: userId);
      
    return p;
  }
}