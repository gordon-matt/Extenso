namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Lookup table containing the types of business entity contacts.
/// </summary>
public partial class ContactType
{
    /// <summary>
    /// Primary key for ContactType records.
    /// </summary>
    public int ContactTypeId { get; set; }

    /// <summary>
    /// Contact type description.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<BusinessEntityContact> BusinessEntityContacts { get; } = new List<BusinessEntityContact>();
}