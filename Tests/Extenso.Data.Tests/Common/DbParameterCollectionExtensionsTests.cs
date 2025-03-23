using Extenso.Data.Common;
using Microsoft.Data.SqlClient;

namespace Extenso.Data.Tests.Common;

public class DbParameterCollectionExtensionsTests
{
    [Fact]
    public void EnsureDbNulls()
    {
        var dbParameters = new[]
        {
            new SqlParameter("Foo", null),
            new SqlParameter("Bar", DBNull.Value),
            new SqlParameter("Baz", null)
        };

        dbParameters.EnsureDbNulls();

        Assert.DoesNotContain(dbParameters, x => x.Value == null);
    }

    [Fact]
    public void EnsureDbNulls_DbParameterCollection()
    {
        using var sqlConnection = new SqlConnection();
        using var cmd = sqlConnection.CreateCommand();
        cmd.Parameters.AddRange(new[]

        {
            new SqlParameter("Foo", null),
            new SqlParameter("Bar", DBNull.Value),
            new SqlParameter("Baz", null)
        });

        cmd.Parameters.EnsureDbNulls();

        Assert.DoesNotContain(cmd.Parameters.OfType<SqlParameter>(), x => x.Value == null);
    }
}