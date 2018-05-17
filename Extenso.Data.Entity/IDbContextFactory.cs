using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity
{
    public interface IDbContextFactory
    {
        DbContext GetContext();
    }
}