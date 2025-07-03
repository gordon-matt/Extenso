using System.Data.Common;

namespace Extenso.Data.Entity;

public class ContextOptions
{
    public int? CommandTimeout { get; set; }

    public DbTransaction Transaction { get; set; }
}