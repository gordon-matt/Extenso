using System.Data.OleDb;
using Microsoft.Extensions.Configuration;

namespace Extenso.Data.OleDb.Tests
{
    public class OleDbConnectionExtensionsTests
    {
        private string connectionString;

        public OleDbConnectionExtensionsTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            connectionString = config.GetConnectionString("DefaultConnection");
        }

        [Fact]
        public void GetRowCount()
        {
            using var connection = new OleDbConnection(connectionString);
            connection.Open();
            int rowCount = connection.GetRowCount("Companies");
            connection.Close();
            Assert.True(rowCount > 0);
        }

        [Fact]
        public void GetTableNames()
        {
            using var connection = new OleDbConnection(connectionString);
            connection.Open();
            var tables = connection.GetTableNames(includeViews: true);
            connection.Close();
            Assert.True(tables.Count() > 0);
        }

        [Fact]
        public void GetForeignKeyData()
        {
            using var connection = new OleDbConnection(connectionString);
            connection.Open();
            var info = connection.GetForeignKeyData("Companies");
            connection.Close();
            Assert.True(info.Count > 0);
        }
    }
}