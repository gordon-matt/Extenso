using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity
{
    public interface IDbContextFactory
    {
        DbContext GetContext();

        DbContext GetContext(string connectionString);
    }
}