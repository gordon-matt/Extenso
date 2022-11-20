using System.Data.SqlClient;
using Extenso.Data.Common;
using Extenso.Data.Tests.Data.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Npgsql;

namespace Extenso.Data.Tests.Common
{
    public class DbConnectionExtensionsTests : IDisposable
    {
        private bool isDisposed;

        private readonly SqlConnection sqlConnection;

        public DbConnectionExtensionsTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            sqlConnection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        [Fact]
        public void CreateParameter()
        {
            var expected = new SqlParameter
            {
                ParameterName = "Foo",
                Value = 999
            };

            var actual = sqlConnection.CreateParameter("Foo", 999);

            Assert.True(actual != null, "Parameter was null");
            Assert.True(actual is SqlParameter, "Parameter was not of the expected type");
            Assert.Equal(actual.ParameterName, expected.ParameterName);
            Assert.Equal(actual.Value, expected.Value);
        }

        [Fact]
        public void ExecuteScalar_Int()
        {
            int expected = 19972;
            int actual = sqlConnection.ExecuteScalar("SELECT COUNT(*) FROM [Person].[Person]");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExecuteScalar_String()
        {
            string expected = "Sánchez";
            string actual = sqlConnection.ExecuteScalar<string>("SELECT [LastName] FROM [Person].[Person] WHERE [BusinessEntityID] = 1");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Insert()
        {
            var rowGuid = Guid.NewGuid();

            var entity = new BusinessEntity
            {
                rowguid = rowGuid,
                ModifiedDate = DateTime.Today
            };

            int rowsAffected = sqlConnection.Insert(entity, "[Person].[BusinessEntity]", new Dictionary<string, string>
            {
                { "rowguid", "rowguid" },
                { "ModifiedDate", "ModifiedDate" }
            });

            Assert.True(rowsAffected == 1, "Unable to insert new entity.");

            rowsAffected = sqlConnection.ExecuteNonQuery(
                "DELETE FROM [Person].[BusinessEntity] WHERE [rowguid] = @rowguid",
                sqlConnection.CreateParameter("rowguid", rowGuid));

            Assert.True(rowsAffected == 1, "Unable to delete the new entity.");
        }

        [Fact]
        public void ExecuteStoredProcedure()
        {
            var dataSet = sqlConnection.ExecuteStoredProcedure("[dbo].[uspGetEmployeeManagers]", new[]
            {
                sqlConnection.CreateParameter("BusinessEntityID", 3)
            });

            Assert.True(dataSet.Tables.Count == 1, "There should be 1 table in the data set.");

            var table = dataSet.Tables[0];

            Assert.True(table.Rows.Count == 1, "There should be 1 row in the table.");
        }

        [Fact]
        public void GetDbProviderFactory_SqlConnection()
        {
            var provider = sqlConnection.GetDbProviderFactory();

            Assert.True(provider != null, "Provider was null");
            Assert.True(provider is SqlClientFactory, "Provider was not of the expected type");
        }

        [Fact]
        public void GetDbProviderFactory_MySqlConnection()
        {
            using var mySqlConnection = new MySqlConnection();
            var provider = mySqlConnection.GetDbProviderFactory();

            Assert.True(provider != null, "Provider was null");
            Assert.True(provider is MySqlClientFactory, "Provider was not of the expected type");
        }

        [Fact]
        public void GetDbProviderFactory_NpgsqlConnection()
        {
            using var npgsqlConnection = new NpgsqlConnection();
            var provider = npgsqlConnection.GetDbProviderFactory();

            Assert.True(provider != null, "Provider was null");
            Assert.True(provider is NpgsqlFactory, "Provider was not of the expected type");
        }

        [Fact]
        public void Validate_ShouldPass()
        {
            bool isValidConnection = sqlConnection.Validate();
            Assert.True(isValidConnection);
        }

        [Fact]
        public void Validate_ShouldFail()
        {
            using var fakeConnection = new SqlConnection();
            bool isValidConnection = fakeConnection.Validate();
            Assert.False(isValidConnection);
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