using System.Data;
using System.Data.Common;
using Extenso.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

public static class DbContextExtensions
{
    public static T ExecuteScalar<T>(this DbContext dbContext, string queryText)
    {
#pragma warning disable DF0010 // A DbConnection provided by the DbContext should NOT be disposed
        var connection = dbContext.Database.GetDbConnection();
#pragma warning restore DF0010

        bool isOpen = connection.State == ConnectionState.Open;

        if (!isOpen)
        {
            connection.Open();
        }

        var result = connection.ExecuteScalar<T>(queryText);

        if (!isOpen)
        {
            connection.Close();
        }

        return result;
    }

    public static DataSet ExecuteStoredProcedure(this DbContext dbContext, string storedProcedure, IEnumerable<DbParameter> parameters)
    {
#pragma warning disable DF0010 // A DbConnection provided by the DbContext should NOT be disposed
        var connection = dbContext.Database.GetDbConnection();
#pragma warning restore DF0010

        bool isOpen = connection.State == ConnectionState.Open;

        if (!isOpen)
        {
            connection.Open();
        }

        var result = connection.ExecuteStoredProcedure(storedProcedure, parameters);

        if (!isOpen)
        {
            connection.Close();
        }

        return result;
    }

    public static DataSet ExecuteStoredProcedure(this DbContext dbContext, string storedProcedure, IEnumerable<DbParameter> parameters, out Dictionary<string, object> outputValues)
    {
#pragma warning disable DF0010 // A DbConnection provided by the DbContext should NOT be disposed
        var connection = dbContext.Database.GetDbConnection();
#pragma warning restore DF0010

        bool isOpen = connection.State == ConnectionState.Open;

        if (!isOpen)
        {
            connection.Open();
        }

        var result = connection.ExecuteStoredProcedure(storedProcedure, parameters, out outputValues);

        if (!isOpen)
        {
            connection.Close();
        }

        return result;
    }

    public static int ExecuteNonQueryStoredProcedure(this DbContext dbContext, string storedProcedure, IEnumerable<DbParameter> parameters)
    {
#pragma warning disable DF0010 // A DbConnection provided by the DbContext should NOT be disposed
        var connection = dbContext.Database.GetDbConnection();
#pragma warning restore DF0010

        bool isOpen = connection.State == ConnectionState.Open;

        if (!isOpen)
        {
            connection.Open();
        }

        int result = connection.ExecuteNonQueryStoredProcedure(storedProcedure, parameters);

        if (!isOpen)
        {
            connection.Close();
        }

        return result;
    }

    public static DbParameter CreateParameter(this DbContext dbContext, string parameterName, object value)
    {
#pragma warning disable DF0010 // A DbConnection provided by the DbContext should NOT be disposed
        var connection = dbContext.Database.GetDbConnection();
#pragma warning restore DF0010

        return connection.CreateParameter(parameterName, value);
    }
}