using System.Data;
using Dapper;
using Npgsql;
using MillionsOfThings.Lib.Services;

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
      //NOTE: Do not instantiate the connection or transaction objects here. This is mostly for the purposes of DI.
    }

    //Primary constructor
    protected BaseRepository(IAppConfiguration configuration) => ConnectionString = configuration.GetConnectionString();

    //Used with the Transaction Manager
    protected BaseRepository(NpgsqlConnection connection)
    {
      Connection = connection;

      ConnectionString = Connection.ConnectionString;
    }

    protected BaseRepository(NpgsqlTransaction transaction)
      : this(transaction.Connection)
      => Transaction = transaction;

    public void Dispose()
    {
      if (Connection == null) return;

      //What happens if a transaction is not committed yet?
      Transaction?.Dispose();

      //The close and/or dispose method more than likely handle the closing if the connection is open
      Connection.Close();
      Connection.Dispose();
    }

    protected NpgsqlConnection GetConnection()
    {
      if (Connection == null || string.IsNullOrWhiteSpace(Connection.ConnectionString))
        Connection = new NpgsqlConnection(ConnectionString);

      if (Connection.State != ConnectionState.Open) Connection.Open();

      return Connection;
    }

    //Not crazy about this
    public void SetTransaction(NpgsqlTransaction transaction)
    {
      Transaction = transaction;
      Connection = Transaction.Connection;

      ConnectionString = Connection.ConnectionString;
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
  }
}
