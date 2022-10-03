using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Connection;
using NHibernate.Driver;
using Npgsql;

namespace CastleSample;

public class InjectedConnectionProvider : ConnectionProvider
{
    private readonly NpgsqlConnection _dbConnection;

    public InjectedConnectionProvider(NpgsqlConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public override void Configure(IDictionary<string, string> settings)
    {
        ConfigureDriver(settings);
    }

    public override DbConnection GetConnection()
    {
        _dbConnection.Open();
        return _dbConnection;
    }
}