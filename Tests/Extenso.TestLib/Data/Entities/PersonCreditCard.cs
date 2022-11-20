namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Cross-reference table mapping people to their credit card information in the CreditCard table.
/// </summary>
public partial class PersonCreditCard
{
    /// <summary>
    /// Business entity identification number. Foreign key to Person.BusinessEntityID.
    /// </summary>
    public int BusinessEntityId { get; set; }

    /// <summary>
    /// Credit card identification number. Foreign key to CreditCard.CreditCardID.
    /// </summary>
    public int CreditCardId { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual Person BusinessEntity { get; set; }

    public virtual CreditCard CreditCard { get; set; }
}