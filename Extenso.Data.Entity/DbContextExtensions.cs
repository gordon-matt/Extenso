using System.Data;
using System.Data.Common;
using Extenso.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

/// <summary>
/// Provides extension methods for database contexts.
/// </summary>
public static class DbContextExtensions
{
    extension(DbContext context)
    {
        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="queryText">The query text.</param>
        /// <returns>The scalar.</returns>
        public T ExecuteScalar<T>(string queryText)
        {
#pragma warning disable DF0010 // A DbConnection provided by the DbContext should NOT be disposed
            var connection = context.Database.GetDbConnection();
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

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The stored procedure.</returns>
        public DataSet ExecuteStoredProcedure(string storedProcedure, IEnumerable<DbParameter> parameters)
        {
#pragma warning disable DF0010 // A DbConnection provided by the DbContext should NOT be disposed
            var connection = context.Database.GetDbConnection();
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

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="outputValues">When this method returns, contains the output values.</param>
        /// <returns>The stored procedure.</returns>
        public DataSet ExecuteStoredProcedure(string storedProcedure, IEnumerable<DbParameter> parameters, out Dictionary<string, object> outputValues)
        {
#pragma warning disable DF0010 // A DbConnection provided by the DbContext should NOT be disposed
            var connection = context.Database.GetDbConnection();
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

        /// <summary>
        /// Executes the non query stored procedure.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The non query stored procedure.</returns>
        public int ExecuteNonQueryStoredProcedure(string storedProcedure, IEnumerable<DbParameter> parameters)
        {
#pragma warning disable DF0010 // A DbConnection provided by the DbContext should NOT be disposed
            var connection = context.Database.GetDbConnection();
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

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="value">The value.</param>
        /// <returns>The parameter.</returns>
        public DbParameter CreateParameter(string parameterName, object value)
        {
#pragma warning disable DF0010 // A DbConnection provided by the DbContext should NOT be disposed
            var connection = context.Database.GetDbConnection();
#pragma warning restore DF0010

            return connection.CreateParameter(parameterName, value);
        }
    }
}