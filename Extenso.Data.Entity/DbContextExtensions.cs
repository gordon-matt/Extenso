using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Extenso.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

public static class DbContextExtensions
{
    public static T ExecuteScalar<T>(this DbContext dbContext, string queryText)
    {
        var connection = dbContext.Database.GetDbConnection();
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
        var connection = dbContext.Database.GetDbConnection();
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
        var connection = dbContext.Database.GetDbConnection();
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
        var connection = dbContext.Database.GetDbConnection();
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

    public static DbParameter CreateParameter(this DbContext dbContext, string parameterName, object value) => dbContext.Database.GetDbConnection().CreateParameter(parameterName, value);
}