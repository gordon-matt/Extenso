using Extenso.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Extenso.Data.Tests.Common
{
    public class DbDataReaderExtensionsTests : IDisposable
    {
        private bool isDisposed;

        private readonly SqlConnection sqlConnection;

        public DbDataReaderExtensionsTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            sqlConnection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        [Fact]
        public void GetBooleanNullable()
        {
            using var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 [HomeOwnerFlag] FROM [Sales].[vPersonDemographics]";
            sqlConnection.Open();
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var exception = Record.Exception(() => reader.GetBooleanNullable(0));
            Assert.Null(exception);
        }

        [Fact]
        public void GetByteNullable()
        {
            using var cmd = sqlConnection.CreateCommand();

            // Unfortunately AdventureWorks2019 does not have a nullable tinyint column in any table. This non-nullable one will do
            cmd.CommandText = "SELECT TOP 1 [TaxType] FROM [Sales].[SalesTaxRate]";
            sqlConnection.Open();
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var exception = Record.Exception(() => reader.GetByteNullable(0));
            Assert.Null(exception);
        }

        [Fact]
        public void GetCharNullable()
        {
            using var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 [SizeUnitMeasureCode] FROM [Production].[Product]";
            sqlConnection.Open();
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var exception = Record.Exception(() => reader.GetCharNullable(0));
            Assert.Null(exception);
        }

        [Fact]
        public void GetDateTimeNullable()
        {
            using var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 [SellEndDate] FROM [Production].[Product]";
            sqlConnection.Open();
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var exception = Record.Exception(() => reader.GetDateTimeNullable(0));
            Assert.Null(exception);
        }

        [Fact]
        public void GetDecimalNullable()
        {
            using var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 [Weight] FROM [Production].[Product]";
            sqlConnection.Open();
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var exception = Record.Exception(() => reader.GetDecimalNullable(0));
            Assert.Null(exception);
        }

        [Fact]
        public void GetDoubleNullable()
        {
            using var cmd = sqlConnection.CreateCommand();

            // Unfortunately AdventureWorks2019 does not have a float or real column in any table. This decimal one will do
            cmd.CommandText = "SELECT TOP 1 [Weight] FROM [Production].[Product]";
            sqlConnection.Open();
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var exception = Record.Exception(() => reader.GetDoubleNullable(0));
            Assert.Null(exception);
        }

        [Fact]
        public void GetFloatNullable()
        {
            using var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 [Weight] FROM [Production].[Product]";
            sqlConnection.Open();
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var exception = Record.Exception(() => reader.GetFloatNullable(0));
            Assert.Null(exception);
        }

        [Fact]
        public void GetGuidNullable()
        {
            using var cmd = sqlConnection.CreateCommand();

            // Unfortunately AdventureWorks2019 does not have a nullable uniqueidentifier column in any table. This non-nullable one will do
            cmd.CommandText = "SELECT TOP 1 [rowguid] FROM [Production].[Product]";
            sqlConnection.Open();
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var exception = Record.Exception(() => reader.GetGuidNullable(0));
            Assert.Null(exception);
        }

        [Fact]
        public void GetInt16Nullable()
        {
            using var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 [ScrapReasonID] FROM [Production].[WorkOrder]";
            sqlConnection.Open();
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var exception = Record.Exception(() => reader.GetInt16Nullable(0));
            Assert.Null(exception);
        }

        [Fact]
        public void GetInt32Nullable()
        {
            using var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 [ProductSubcategoryID] FROM [Production].[Product]";
            sqlConnection.Open();
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var exception = Record.Exception(() => reader.GetInt32Nullable(0));
            Assert.Null(exception);
        }

        [Fact]
        public void GetInt64Nullable()
        {
            using var cmd = sqlConnection.CreateCommand();

            // Unfortunately AdventureWorks2019 does not have a bigint column in any table. This int one will do
            cmd.CommandText = "SELECT TOP 1 [ProductSubcategoryID] FROM [Production].[Product]";
            sqlConnection.Open();
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var exception = Record.Exception(() => reader.GetInt64Nullable(0));
            Assert.Null(exception);
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