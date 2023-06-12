using System.Data;
using Extenso.Collections;
using Extenso.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Extenso.Data.Tests.SqlClient
{
    public class SqlConnectionExtensionsTests : IDisposable
    {
        private bool isDisposed;

        private readonly SqlConnection sqlConnection;

        public SqlConnectionExtensionsTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            sqlConnection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        [Fact]
        public void GetColumnData()
        {
            var columns = sqlConnection.GetColumnData("EmailAddress", "Person");

            var column = columns["EmailAddress"];
            Assert.True(column.IsNullable, "'EmailAddress' should be nullable.");
            Assert.True(column.DataType == DbType.String, "'EmailAddress' should have a data type of DbType.String.");
            Assert.True(column.DataTypeNative == "nvarchar", "'EmailAddress' should have a native data type of 'nvarchar'.");
            Assert.True(column.OrdinalPosition == 3, "'EmailAddress' should have an ordinal position of 3.");

            column = columns["BusinessEntityID"];
            Assert.True(column.KeyType == KeyType.PrimaryKey, "'BusinessEntityID' should be a foreign key.");
        }

        [Fact]
        public void GetDatabaseNames()
        {
            var databaseNames = sqlConnection.GetDatabaseNames();
            Assert.Contains(databaseNames, x => x == "AdventureWorks2019");
        }

        [Fact]
        public void GetSchemaNames()
        {
            var schemaNames = sqlConnection.GetSchemaNames();
            Assert.True(schemaNames.ContainsAll("dbo", "HumanResources", "Person", "Production", "Purchasing", "Sales"));
        }

        [Fact]
        public void GetRowCount()
        {
            int expected = 19972;
            var actual = sqlConnection.GetRowCount("Person", "Person");

            Assert.Equal(expected, actual);
        }

        #region Dispose Pattern

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    sqlConnection?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                isDisposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DbConnectionExtensionsTests()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose Pattern
    }
}