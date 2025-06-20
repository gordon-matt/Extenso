﻿using System.Data;
using System.Data.Common;
using System.Reflection;
using Extenso.Collections;
using Microsoft.Data.SqlClient;

namespace Extenso.Data.Common;

/// <summary>
/// Provides a set of static methods for System.Data.Common.DbConnection.
/// </summary>
public static class DbConnectionExtensions
{
    /// <summary>
    /// Returns a new instance of the provider's class that implements the System.Data.Common.DbParameter class
    /// and sets both the name and value from the given parameters.
    /// </summary>
    /// <param name="connection">A DbConnection for which to create the DbParameter.</param>
    /// <param name="parameterName">The name to set for the parameter.</param>
    /// <param name="value">The value to set for the parameter.</param>
    /// <returns>A new instance of System.Data.Common.DbParameter.</returns>
    public static DbParameter CreateParameter(this DbConnection connection, string parameterName, object value)
    {
        var param = GetDbProviderFactory(connection).CreateParameter();
        param.ParameterName = parameterName;
        param.Value = value;
        return param;
    }

    /// <summary>
    /// Executes the query and returns the first column of the first row in the result
    /// set returned by the query. All other columns and rows are ignored.
    /// This overload assumes the return type is an integer.
    /// </summary>
    /// <param name="connection">A DbConnection to execute [queryText] upon.</param>
    /// <param name="queryText">The T-SQL statement to execute.</param>
    /// <returns>The first column of the first row in the result set.</returns>
    public static int ExecuteScalar(this DbConnection connection, string queryText, params DbParameter[] parameters)
    {
        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        return connection.ExecuteScalar<int>(queryText, parameters);
    }

    /// <summary>
    /// Executes the query and returns the first column of the first row in the result
    /// set returned by the query. All other columns and rows are ignored.
    /// A generic parameter specifies the return type.
    /// </summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="connection">A DbConnection to execute [queryText] upon.</param>
    /// <param name="queryText">The T-SQL statement to execute.</param>
    /// <returns>The first column of the first row in the result set.</returns>
    public static T ExecuteScalar<T>(this DbConnection connection, string queryText, params DbParameter[] parameters)
    {
        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using var command = connection.CreateCommand();
        command.CommandTimeout = 300;
        command.CommandType = CommandType.Text;
        command.CommandText = queryText;

        if (!parameters.IsNullOrEmpty())
        {
            command.Parameters.AddRange(parameters);
            command.Parameters.EnsureDbNulls();
        }

        return (T)command.ExecuteScalar();
    }

    /// <summary>
    /// Executes a command against the given connection object, returning the number of rows affected.
    /// </summary>
    /// <param name="connection">A DbConnection to execute [queryText] upon.</param>
    /// <param name="queryText">The T-SQL statement to execute.</param>
    /// <param name="parameters"></param>
    /// <returns>The number of rows affected.</returns>
    public static int ExecuteNonQuery(this DbConnection connection, string queryText, params DbParameter[] parameters)
    {
        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        using var command = connection.CreateCommand();
        command.CommandTimeout = 300;
        command.CommandType = CommandType.Text;
        command.CommandText = queryText;

        if (!parameters.IsNullOrEmpty())
        {
            command.Parameters.AddRange(parameters);
            command.Parameters.EnsureDbNulls();
        }

        return command.ExecuteNonQuery();
    }

    /// <summary>
    /// Gets the associated provider factory for the given System.Data.Common.DbConnection.
    /// </summary>
    /// <param name="connection">A DbConnection for which to find the associated provider factory.</param>
    /// <returns>An instance of the provider factory associated with the given connection.</returns>
    public static DbProviderFactory GetDbProviderFactory(this DbConnection connection) => connection is SqlConnection ? SqlClientFactory.Instance : DbProviderFactories.GetFactory(connection);// Only use reflection as last option//return (DbProviderFactory)connection.GetPrivatePropertyValue("DbProviderFactory");

    /// <summary>
    ///
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="storedProcedure"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static DataSet ExecuteStoredProcedure(this DbConnection connection, string storedProcedure, IEnumerable<DbParameter> parameters) => connection.ExecuteStoredProcedure(storedProcedure, parameters, out _);

    /// <summary>
    ///
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="storedProcedure"></param>
    /// <param name="parameters"></param>
    /// <param name="outputValues"></param>
    /// <returns></returns>
    public static DataSet ExecuteStoredProcedure(
        this DbConnection connection, string storedProcedure, IEnumerable<DbParameter> parameters, out Dictionary<string, object> outputValues)
    {
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = storedProcedure;
        parameters.ForEach(p => command.Parameters.Add(p));
        command.Parameters.EnsureDbNulls();
        var dataSet = new DataSet();

        var factory = connection.GetDbProviderFactory();
        using (var adapter = factory.CreateDataAdapter())
        {
            adapter.SelectCommand = command;
            adapter.Fill(dataSet);
        }

        outputValues = [];

        foreach (DbParameter param in command.Parameters)
        {
            if (param.Direction == ParameterDirection.Output)
            {
                outputValues.Add(param.ParameterName, param.Value);
            }
        }

        return dataSet;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="storedProcedure"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static int ExecuteNonQueryStoredProcedure(this DbConnection connection, string storedProcedure, IEnumerable<DbParameter> parameters)
    {
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = storedProcedure;
        parameters.ForEach(p => command.Parameters.Add(p));
        command.Parameters.EnsureDbNulls();

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        int rowsAffected = command.ExecuteNonQuery();

        if (!alreadyOpen)
        {
            connection.Close();
        }

        return rowsAffected;
    }

    /// <summary>
    /// Inserts [entity] into the specified database table. The object's property names are matched to column names
    /// by using the specified mappings dictionary.
    /// </summary>
    /// <typeparam name="T">The type of entity to persist to the database.</typeparam>
    /// <param name="connection">The DbConnection to use.</param>
    /// <param name="entity">The entity to persist to the database.</param>
    /// <param name="tableName">The name of the table to insert the entity into.</param>
    /// <param name="mappings">
    /// A System.Collection.Generic.IDictionary`2 used to map object properties to column names.
    /// Key = Property Name, Value = Column Name.
    /// </param>
    /// <returns>The number of rows affected.</returns>
    public static int Insert<T>(this DbConnection connection, T entity, string tableName, string schema = null, IDictionary<string, string> mappings = null)
    {
        if (mappings.IsNullOrEmpty())
        {
            mappings = typeof(T).GetTypeInfo().GetProperties()
                .ToDictionary(k => k.Name, v => v.Name);
        }

        const string INSERT_INTO_FORMAT = "INSERT INTO {0}({1}) VALUES({2})";
        var parameterNames = CreateParameterNames(mappings.Values);

        using var commandBuilder = connection.GetDbProviderFactory().CreateCommandBuilder();
        string fieldNames = parameterNames.Keys.Select(commandBuilder.QuoteIdentifier).Join(",");
        var properties = typeof(T).GetTypeInfo().GetProperties();

        using var command = connection.CreateCommand();
        string commandText = string.Format(INSERT_INTO_FORMAT, GetFullTableName(connection, tableName, schema), fieldNames, parameterNames.Values.Join(","));
        command.CommandType = CommandType.Text;
        command.CommandText = commandText;

        mappings.ForEach(mapping =>
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterNames[mapping.Value];
            var property = properties.Single(p => p.Name == mapping.Key);
            parameter.DbType = DataTypeConvertor.GetDbType(property.PropertyType);
            parameter.Value = GetFormattedValue(property.PropertyType, property.GetValue(entity, null));
            command.Parameters.Add(parameter);
        });

        bool alreadyOpen = connection.State != ConnectionState.Closed;

        if (!alreadyOpen)
        {
            connection.Open();
        }

        int rowsAffected = command.ExecuteNonQuery();

        if (!alreadyOpen)
        {
            connection.Close();
        }

        return rowsAffected;
    }

    /// <summary>
    /// Inserts [entities] into the specified database table. The objects' property names are matched to column names
    /// by using the specified mappings dictionary.
    /// </summary>
    /// <typeparam name="T">The type of entity to persist to the database.</typeparam>
    /// <param name="connection">The DbConnection to use.</param>
    /// <param name="entities">The entities to persist to the database.</param>
    /// <param name="tableName">The name of the table to insert the entity into.</param>
    /// <param name="mappings">
    /// A System.Collection.Generic.IDictionary`2 used to map object properties to column names.
    /// Key = Property Name, Value = Column Name.
    /// </param>
    /// <returns>The number of rows affected.</returns>
    public static int InsertCollection<T>(this DbConnection connection, IEnumerable<T> entities, string tableName, string schema = null, IDictionary<string, string> mappings = null)
    {
        if (mappings.IsNullOrEmpty())
        {
            mappings = typeof(T).GetTypeInfo().GetProperties()
                .ToDictionary(k => k.Name, v => v.Name);
        }

        const string INSERT_INTO_FORMAT = "INSERT INTO {0}({1}) VALUES({2})";
        var parameterNames = CreateParameterNames(mappings.Values);

        using var commandBuilder = connection.GetDbProviderFactory().CreateCommandBuilder();
        string fieldNames = parameterNames.Keys.Select(commandBuilder.QuoteIdentifier).Join(",");
        var properties = typeof(T).GetTypeInfo().GetProperties();

        using var command = connection.CreateCommand();
        string commandText = string.Format(INSERT_INTO_FORMAT, GetFullTableName(connection, tableName, schema), fieldNames, parameterNames.Values.Join(","));
        command.CommandType = CommandType.Text;
        command.CommandText = commandText;

        mappings.ForEach(mapping =>
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterNames[mapping.Value];
            var property = properties.Single(p => p.Name == mapping.Key);
            parameter.DbType = DataTypeConvertor.GetDbType(property.PropertyType);
            command.Parameters.Add(parameter);
        });

        bool alreadyOpen = connection.State != ConnectionState.Closed;
        if (!alreadyOpen)
        {
            connection.Open();
        }

        int rowsAffected = 0;
        using var transaction = connection.BeginTransaction();
        command.Transaction = transaction;

        try
        {
            foreach (var entity in entities)
            {
                properties.ForEach(property =>
                {
                    if (mappings.ContainsKey(property.Name))
                    {
                        command.Parameters[parameterNames[mappings[property.Name]]].Value =
                            GetFormattedValue(property.PropertyType, property.GetValue(entity, null));
                    }
                });
                rowsAffected += command.ExecuteNonQuery();
            }
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            if (!alreadyOpen)
            {
                connection.Close();
            }
        }

        return rowsAffected;
    }

    /// <summary>
    /// Inserts rows from [table] into the specified database table. The table's column names are matched to database column names
    /// by using the specified mappings dictionary.
    /// </summary>
    /// <param name="connection">The DbConnection to use.</param>
    /// <param name="table">The data to persist to the database.</param>
    /// <param name="tableName">The name of the table to insert the entity into.</param>
    /// <param name="mappings">
    /// A System.Collection.Generic.IDictionary`2 used to map object properties to column names.
    /// Key = Property Name, Value = Column Name.
    /// </param>
    /// <returns>The number of rows affected.</returns>
    public static int InsertDataTable(this DbConnection connection, DataTable table, string tableName, string schema = null, IDictionary<string, string> mappings = null)
    {
        if (mappings.IsNullOrEmpty())
        {
            mappings = table.Columns.OfType<DataColumn>()
                .ToDictionary(k => k.ColumnName, v => v.ColumnName);
        }

        const string INSERT_INTO_FORMAT = "INSERT INTO {0}({1}) VALUES({2})";
        var parameterNames = CreateParameterNames(mappings.Values);

        using var commandBuilder = connection.GetDbProviderFactory().CreateCommandBuilder();
        string fieldNames = parameterNames.Keys.Select(commandBuilder.QuoteIdentifier).Join(",");

        var columns = table.Columns.OfType<DataColumn>().Select(x => new { x.ColumnName, x.DataType });

        using var command = connection.CreateCommand();
        string commandText = string.Format(INSERT_INTO_FORMAT, GetFullTableName(connection, tableName, schema), fieldNames, parameterNames.Values.Join(","));
        command.CommandType = CommandType.Text;
        command.CommandText = commandText;

        mappings.ForEach(mapping =>
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterNames[mapping.Value];
            var column = columns.Single(x => x.ColumnName == mapping.Key);
            parameter.DbType = DataTypeConvertor.GetDbType(column.DataType);
            command.Parameters.Add(parameter);
        });

        bool alreadyOpen = connection.State != ConnectionState.Closed;
        if (!alreadyOpen)
        {
            connection.Open();
        }

        int rowsAffected = 0;
        using var transaction = connection.BeginTransaction();
        command.Transaction = transaction;

        try
        {
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    if (mappings.ContainsKey(column.ColumnName))
                    {
                        command.Parameters[parameterNames[mappings[column.ColumnName]]].Value = row[column];
                    }
                }
                rowsAffected += command.ExecuteNonQuery();
            }
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            if (!alreadyOpen)
            {
                connection.Close();
            }
        }

        return rowsAffected;
    }

    /// <summary>
    /// Tries to establish a connection.
    /// </summary>
    /// <param name="connection">The DbConnection</param>
    /// <returns>True if successful. Otherwise, false</returns>
    public static bool Validate(this DbConnection connection) => connection.Validate(5);

    /// <summary>
    /// Tries to establish a connection.
    /// </summary>
    /// <param name="connection">The DbConnection</param>
    /// <param name="maxTries">The number of times to try connecting.</param>
    /// <returns>True if successful. Otherwise, false</returns>
    public static bool Validate(this DbConnection connection, byte maxTries)
    {
        try
        {
            bool alreadyOpen = connection.State != ConnectionState.Closed;

            if (!alreadyOpen)
            {
                connection.Open();
            }

            byte numberOfTries = 1;
            while (connection.State == ConnectionState.Connecting && numberOfTries <= maxTries)
            {
                Thread.Sleep(100);
                numberOfTries++;
            }
            bool valid = connection.State == ConnectionState.Open;

            if (!alreadyOpen)
            {
                connection.Close();
            }

            return valid;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
        catch (DbException)
        {
            return false;
        }
    }

    private static object GetFormattedValue(Type type, object value) => value == null
        ? null
        : type.Name switch
        {
            "Boolean" => (bool)value ? 1 : 0,
            "String" => ((string)value).Replace("'", "''"),
            "DBNull" => null,
            _ => value,
        };

    private static IDictionary<string, string> CreateParameterNames(IEnumerable<string> fieldNames)
    {
        var parameterNames = new Dictionary<string, string>();
        fieldNames.ForEach(f =>
        {
            string parameterName = f;
            "¬`!\"£$%^&*()-=+{}[]:;@'~#|<>,.?/ ".ToCharArray().ForEach(c => parameterName = parameterName.Replace(c, '_'));
            parameterNames.Add(f, parameterName.ToPascalCase().Prepend("@"));
        });
        return parameterNames;
    }

    private static string GetFullTableName(DbConnection connection, string tableName, string schema)
    {
        using var commandBuilder = connection.GetDbProviderFactory().CreateCommandBuilder();

        return !string.IsNullOrEmpty(schema)
            ? $"{commandBuilder.QuoteIdentifier(schema)}.{commandBuilder.QuoteIdentifier(tableName)}"
            : commandBuilder.QuoteIdentifier(tableName);
    }
}