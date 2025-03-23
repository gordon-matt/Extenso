namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Unit of measure lookup table.
/// </summary>
public partial class UnitMeasure
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public string UnitMeasureCode { get; set; }

    /// <summary>
    /// Unit of measure description.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<BillOfMaterial> BillOfMaterials { get; } = [];

    public virtual ICollection<Product> ProductSizeUnitMeasureCodeNavigations { get; } = [];

    public virtual ICollection<ProductVendor> ProductVendors { get; } = [];

    public virtual ICollection<Product> ProductWeightUnitMeasureCodeNavigations { get; } = [];
}