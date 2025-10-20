using Dapper;
using MillionsOfThings.Lib.DataAccess.Utility;
using MillionsOfThings.Lib.Services;
using MillionsOfThings.Lib.Services.Utility;
using Npgsql;
using System.Data;
using System.Transactions;

namespace MillionsOfThings.Lib.DataAccess
{
  public abstract class BaseRepository
    : IDisposable
  {
    protected NpgsqlConnection? Connection;

    protected string? ConnectionString;

    protected NpgsqlTransaction? Transaction;

    protected BaseRepository()
    {
      //NOTE: Do not instantiate the connection or transaction objects here.
      //This is here for the purposes of DI.
    }

    //Primary constructor
    protected BaseRepository(IAppConfiguration configuration) => ConnectionString = configuration.GetConnectionString();

    public void Dispose()
    {
      if (Connection == null) return;

      //Setting variables to null to indicate they are no longer usable
      //Doing this in-lieu of having a boolean flag to indicate disposed state

      //What happens if a transaction is not committed yet?
      Transaction?.Dispose();
      Transaction = null;

      //The close and/or dispose method more than likely handles closing the connection if it is open
      Connection.Close();
      Connection.Dispose();
      Connection = null;
    }

    public async Task BeginTransaction()
      => await GetConnection(true);

    public async Task CommitTransaction()
    {
      if (Transaction == null)
        throw new InvalidOperationException("There is no active transaction to commit. 0x202510181911");

      await Transaction.CommitAsync();
      
      Dispose();
    }

    public async Task RollbackTransaction()
    {
      if (Transaction == null)
        throw new InvalidOperationException("There is no active transaction to rollback. 0x202510181914");

      await Transaction.RollbackAsync();

      Dispose();
    }

    public void JoinExistingTransaction(IDbTransaction transaction)
    {
      if (Transaction != null)
        throw new InvalidOperationException("There is already an active transaction for this repository. 0x202510192051");

      if (Connection != null)
        throw new InvalidOperationException("There is already an active connection for this repository. 0x202510192053");
      
      Transaction = transaction as NpgsqlTransaction;
      Connection = Transaction.Connection;
      ConnectionString = Connection.ConnectionString;
    }

    protected async Task<NpgsqlConnection> GetConnection(bool useTransaction = false)
    {
      //If the shared Connection has not been instantiated yet or the
      //shared Connection's ConnectionString is empty, create a new one
      if (Connection == null || string.IsNullOrWhiteSpace(Connection.ConnectionString))
        Connection = new NpgsqlConnection(ConnectionString);
      
      if (Connection.State != ConnectionState.Open) await Connection.OpenAsync();

      if (Transaction == null && useTransaction)
      {
        Transaction = await Connection.BeginTransactionAsync();
      }

      return Connection;
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

      await using var connection = new NpgsqlConnection(ConnectionString);

      var sql = string.Format(updateTemplate, string.Join(", ", lst));

      await connection.ExecuteAsync(sql, p, Transaction);
    }

    protected static DynamicParameters AddUserIdParameter(DynamicParameters p, int userId)
    {
      p.Add("@user_id", dbType: DbType.Int32, value: userId);
      
      return p;
    }
  }
}
