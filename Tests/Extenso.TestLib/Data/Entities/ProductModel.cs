namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Product model classification.
/// </summary>
public partial class ProductModel : IEntity
{
    /// <summary>
    /// Primary key for ProductModel records.
    /// </summary>
    public int ProductModelId { get; set; }

    /// <summary>
    /// Product model description.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Detailed product catalog information in xml format.
    /// </summary>
    public string CatalogDescription { get; set; }

    /// <summary>
    /// Manufacturing instructions in xml format.
    /// </summary>
    public string Instructions { get; set; }

    /// <summary>
    /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
    /// </summary>
    public Guid Rowguid { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public object[] KeyValues => new object[] { ProductModelId };

    public virtual ICollection<ProductModelIllustration> ProductModelIllustrations { get; } = new List<ProductModelIllustration>();

    public virtual ICollection<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultures { get; } = new List<ProductModelProductDescriptionCulture>();

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}