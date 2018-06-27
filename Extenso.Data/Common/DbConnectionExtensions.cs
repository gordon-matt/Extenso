using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading;
using Extenso.Collections;
using Extenso.Reflection;

namespace Extenso.Data.Common
{
    public static class DbConnectionExtensions
    {
        public static DbParameter CreateParameter(this DbConnection connection, string parameterName, object value)
        {
            var param = GetDbProviderFactory(connection).CreateParameter();
            param.ParameterName = parameterName;
            param.Value = value;
            return param;
        }

        public static int ExecuteScalar(this DbConnection connection, string queryText)
        {
            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            return connection.ExecuteScalar<int>(queryText);
        }

        public static T ExecuteScalar<T>(this DbConnection connection, string queryText)
        {
            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = queryText;
                command.CommandTimeout = 300;
                return (T)command.ExecuteScalar();
            }
        }

        public static DbProviderFactory GetDbProviderFactory(this DbConnection connection)
        {
            if (connection is SqlConnection)
            {
                return SqlClientFactory.Instance;
            }

            // Only use reflection as last option
            // NOTE: Not sure if this still works with .NET Core, as it did with the old .NET Framework
            return (DbProviderFactory)connection.GetPrivatePropertyValue("DbProviderFactory");

            // This would be best, but currently it has not been implemented for .NET Core.
            //  For more info, see: https://github.com/dotnet/corefx/issues/20903
            //return DbProviderFactories.GetFactory(connection);
        }

        public static DataSet ExecuteStoredProcedure(this DbConnection connection, string storedProcedure, IEnumerable<DbParameter> parameters)
        {
            Dictionary<string, object> outputValues;
            return connection.ExecuteStoredProcedure(storedProcedure, parameters, out outputValues);
        }

        public static DataSet ExecuteStoredProcedure(this DbConnection connection, string storedProcedure, IEnumerable<DbParameter> parameters, out Dictionary<string, object> outputValues)
        {
            using (var command = connection.CreateCommand())
            {
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

                outputValues = new Dictionary<string, object>();

                foreach (DbParameter param in command.Parameters)
                {
                    if (param.Direction == ParameterDirection.Output)
                    {
                        outputValues.Add(param.ParameterName, param.Value);
                    }
                }

                return dataSet;
            }
        }

        public static int ExecuteNonQueryStoredProcedure(this DbConnection connection, string storedProcedure, IEnumerable<DbParameter> parameters)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storedProcedure;
                parameters.ForEach(p => command.Parameters.Add(p));
                command.Parameters.EnsureDbNulls();

                bool alreadyOpen = (connection.State != ConnectionState.Closed);

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
        }

        /// <summary>
        /// <para>Inserts the specified entity into the specified Table. Property names are used to match</para>
        /// <para>with Sql Column Names.</para>
        /// </summary>
        /// <typeparam name="T">The type of entity to persist to Sql database.</typeparam>
        /// <param name="connection">This DbConnection.</param>
        /// <param name="entity">The entity to persist to Sql database.</param>
        /// <param name="tableName">The table to insert the entity into.</param>
        /// <returns>Number of rows affected.</returns>
        public static int Insert<T>(this DbConnection connection, T entity, string tableName)
        {
            var mappings = typeof(T).GetTypeInfo().GetProperties()
                .ToDictionary(k => k.Name, v => v.Name);

            return connection.Insert(entity, tableName, mappings);
        }

        /// <summary>
        /// <para>Inserts the specified entity into the specified Table. Properties are matched</para>
        /// <para>with Sql Column Names by using the specified mappings dictionary.</para>
        /// </summary>
        /// <typeparam name="T">The type of entity to persist to Sql database.</typeparam>
        /// <param name="connection">This DbConnection.</param>
        /// <param name="entity">The entity to persist to Sql database.</param>
        /// <param name="tableName">The table to insert the entity into.</param>
        /// <param name="mappings">
        ///     <para>A Dictionary to use to map properties to Sql columns.</para>
        ///     <para>Key = Property Name, Value = Sql Column Name.</para>
        /// </param>
        /// <returns>Number of rows affected.</returns>
        public static int Insert<T>(this DbConnection connection, T entity, string tableName, IDictionary<string, string> mappings)
        {
            const string INSERT_INTO_FORMAT = "INSERT INTO {0}({1}) VALUES({2})";
            string fieldNames = mappings.Values.Join(",");
            string parameterNames = fieldNames.Replace(",", ",@").Prepend("@");

            var properties = typeof(T).GetTypeInfo().GetProperties();

            using (var command = connection.CreateCommand())
            {
                string commandText = string.Format(INSERT_INTO_FORMAT, tableName, fieldNames, parameterNames);
                command.CommandType = CommandType.Text;
                command.CommandText = commandText;

                mappings.ForEach(mapping =>
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = string.Concat("@", mapping.Value);
                    var property = properties.Single(p => p.Name == mapping.Key);
                    parameter.DbType = DataTypeConvertor.GetDbType(property.GetType());
                    parameter.Value = GetFormattedValue(property.PropertyType, property.GetValue(entity, null));
                    command.Parameters.Add(parameter);
                });

                bool alreadyOpen = (connection.State != ConnectionState.Closed);

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
        }

        /// <summary>
        /// <para>Inserts the specified entities into the specified Table. Property names are used to match</para>
        /// <para>with Sql Column Names.</para>
        /// </summary>
        /// <typeparam name="T">The type of entity to persist to Sql database.</typeparam>
        /// <param name="connection">This DbConnection.</param>
        /// <param name="entities">The entities to persist to Sql database.</param>
        /// <param name="tableName">The table to insert the entities into.</param>
        public static void InsertCollection<T>(this DbConnection connection, IEnumerable<T> entities, string tableName)
        {
            var mappings = typeof(T).GetTypeInfo().GetProperties()
                .ToDictionary(k => k.Name, v => v.Name);

            connection.InsertCollection(entities, tableName, mappings);
        }

        /// <summary>
        /// <para>Inserts the specified entities into the specified Table. Properties are matched</para>
        /// <para>with Sql Column Names by using the specified mappings dictionary.</para>
        /// </summary>
        /// <typeparam name="T">The type of entity to persist to Sql database.</typeparam>
        /// <param name="connection">This DbConnection.</param>
        /// <param name="entities">The entities to persist to Sql database.</param>
        /// <param name="tableName">The table to insert the entities into.</param>
        /// <param name="mappings">
        ///     <para>A Dictionary to use to map properties to Sql columns.</para>
        ///     <para>Key = Property Name, Value = Sql Column Name.</para>
        /// </param>
        public static void InsertCollection<T>(this DbConnection connection, IEnumerable<T> entities, string tableName, IDictionary<string, string> mappings)
        {
            const string INSERT_INTO_FORMAT = "INSERT INTO {0}({1}) VALUES({2})";
            string fieldNames = mappings.Values.Join(",");
            string parameterNames = fieldNames.Replace(",", ",@").Prepend("@");

            var properties = typeof(T).GetTypeInfo().GetProperties();

            using (var command = connection.CreateCommand())
            {
                string commandText = string.Format(INSERT_INTO_FORMAT, tableName, fieldNames, parameterNames);
                command.CommandType = CommandType.Text;
                command.CommandText = commandText;

                mappings.ForEach(mapping =>
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = string.Concat("@", mapping.Value);
                    var property = properties.Single(p => p.Name == mapping.Key);
                    parameter.DbType = DataTypeConvertor.GetDbType(property.PropertyType);
                    command.Parameters.Add(parameter);
                });

                bool alreadyOpen = (connection.State != ConnectionState.Closed);

                if (!alreadyOpen)
                {
                    connection.Open();
                }

                foreach (T entity in entities)
                {
                    properties.ForEach(property =>
                    {
                        command.Parameters["@" + property.Name].Value =
                            GetFormattedValue(property.PropertyType, property.GetValue(entity, null));
                    });
                    command.ExecuteNonQuery();
                }

                if (!alreadyOpen)
                {
                    connection.Close();
                }
            }
        }

        public static void InsertDataTable(
            this DbConnection connection,
            DataTable table,
            string tableName,
            IDictionary<string, string> mappings,
            Func<string, string> encloseIdentifier)
        {
            string fieldNames = mappings.Values.Select(x => encloseIdentifier(x)).Join(",");
            string parameterNames = mappings.Values.Join(",").Replace(",", ",@").Prepend("@");

            var columns = table.Columns.OfType<DataColumn>().Select(x => new { x.ColumnName, x.DataType });

            const string INSERT_INTO_FORMAT = "INSERT INTO {0}({1}) VALUES({2})";
            using (var command = connection.CreateCommand())
            {
                string commandText = string.Format(
                    INSERT_INTO_FORMAT,
                    tableName,
                    fieldNames,
                    parameterNames);

                command.CommandType = CommandType.Text;
                command.CommandText = commandText;

                mappings.ForEach(mapping =>
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = string.Concat("@", mapping.Value);
                    var column = columns.Single(x => x.ColumnName == mapping.Key);
                    parameter.DbType = DataTypeConvertor.GetDbType(column.DataType);
                    command.Parameters.Add(parameter);
                });

                bool alreadyOpen = (connection.State != ConnectionState.Closed);

                if (!alreadyOpen)
                {
                    connection.Open();
                }

                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        command.Parameters["@" + column.ColumnName].Value = row[column];

                        //command.Parameters["@" + column.ColumnName].Value =
                        //    GetFormattedValue(column.DataType, row[column]);
                    }
                    command.ExecuteNonQuery();
                }

                //if (!alreadyOpen)
                //{
                //    connection.Close();
                //}
            }
        }

        /// <summary>
        /// Tries to establish a connection.
        /// </summary>
        /// <param name="connection">The DbConnection</param>
        /// <returns>True if successful. Otherwise, false</returns>
        public static bool Validate(this DbConnection connection)
        {
            return connection.Validate(5);
        }

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
                bool alreadyOpen = (connection.State != ConnectionState.Closed);

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

        private static string GetFormattedValue(Type type, object value)
        {
            if (value == null)
            {
                return "NULL";
            }

            switch (type.Name)
            {
                case "Boolean": return (bool)value ? "1" : "0";

                case "String": return ((string)value).Replace("'", "''");
                case "DateTime": return ((DateTime)value).ToISO8601DateString();

                //case "String": return ((string)value).Replace("'", "''").AddSingleQuotes();
                //case "DateTime": return ((DateTime)value).ToISO8601DateString().AddSingleQuotes();

                case "Byte":
                case "Decimal":
                case "Double":
                case "Int16":
                case "Int32":
                case "Int64":
                case "SByte":
                case "Single":
                case "UInt16":
                case "UInt32":
                case "UInt64": return value.ToString();

                case "DBNull": return "NULL";

                default: return value.ToString().EnquoteSingle();
            }
        }
    }
}