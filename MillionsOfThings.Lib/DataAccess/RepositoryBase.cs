﻿using Dapper;
using Microsoft.Data.SqlClient;
using MillionsOfThings.Lib.Services;
using System.Data;

namespace MillionsOfThings.Lib.DataAccess
{
  public abstract class RepositoryBase
    : IDisposable
  {
    protected SqlConnection? Connection;

    protected string? ConnectionString;

    protected SqlTransaction? Transaction;

    protected RepositoryBase()
    {
      //NOTE: Do not instantiate the connection or transaction objects here. This is mostly for the purposes of DI.
    }

    //Primary constructor
    protected RepositoryBase(IAppConfiguration configuration) => ConnectionString = configuration.GetConnectionString();

    //Used with the Transaction Manager
    protected RepositoryBase(SqlConnection connection)
    {
      Connection = connection;

      ConnectionString = Connection.ConnectionString;
    }

    protected RepositoryBase(SqlTransaction transaction)
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

    protected SqlConnection GetConnection()
    {
      if (Connection == null || string.IsNullOrWhiteSpace(Connection.ConnectionString))
        Connection = new SqlConnection(ConnectionString);

      if (Connection.State != ConnectionState.Open) Connection.Open();

      return Connection;
    }

    //Not crazy about this
    public void SetTransaction(SqlTransaction transaction)
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
