﻿namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Product subcategories. See ProductCategory table.
/// </summary>
public partial class ProductSubcategory
{
    /// <summary>
    /// Primary key for ProductSubcategory records.
    /// </summary>
    public int ProductSubcategoryId { get; set; }

    /// <summary>
    /// Product category identification number. Foreign key to ProductCategory.ProductCategoryID.
    /// </summary>
    public int ProductCategoryId { get; set; }

    /// <summary>
    /// Subcategory description.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
    /// </summary>
    public Guid Rowguid { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual ProductCategory ProductCategory { get; set; }

    public virtual ICollection<Product> Products { get; } = [];
}