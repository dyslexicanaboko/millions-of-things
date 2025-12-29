using Dapper;
using MillionsOfThings.Lib.DataAccess.Utility;
using MillionsOfThings.Lib.Services;
using MillionsOfThings.Lib.Services.Utility;
using Npgsql;
using System.Data;

namespace MillionsOfThings.Lib.DataAccess
{
  public abstract class BaseRepository
  {
    protected string? ConnectionString;

    protected BaseRepository()
    {
      //NOTE: Do not instantiate objects here. This is here for the purposes of DI only.
    }

    //Primary constructor
    protected BaseRepository(IAppConfiguration configuration) => ConnectionString = configuration.GetConnectionString();

    protected async Task<NpgsqlConnection> GetConnection()
    {
      if (string.IsNullOrWhiteSpace(ConnectionString)) throw new ApplicationException("Connection string cannot be blank, whitespace, or null.");

      var conn = new NpgsqlConnection(ConnectionString);
      
      await conn.OpenAsync();

      return conn;
    }

    protected SqlMapper.ICustomQueryParameter GetTvpIntegerList(IList<int> integerList)
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

    protected async Task UpdatePartial(
      string updateTemplate,
      List<ColumnSchema> updatePartialColumns,
      DynamicParameters p, 
      IList<UpdateInstruction> instructions)
    {
      if (!instructions.Any()) return;

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
}
