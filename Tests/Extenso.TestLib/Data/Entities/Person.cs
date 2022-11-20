﻿namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Human beings involved with AdventureWorks: employees, customer contacts, and vendor contacts.
/// </summary>
public partial class Person : IEntity
{
    /// <summary>
    /// Primary key for Person records.
    /// </summary>
    public int BusinessEntityId { get; set; }

    /// <summary>
    /// Primary type of person: SC = Store Contact, IN = Individual (retail) customer, SP = Sales person, EM = Employee (non-sales), VC = Vendor contact, GC = General contact
    /// </summary>
    public string PersonType { get; set; }

    /// <summary>
    /// 0 = The data in FirstName and LastName are stored in western style (first name, last name) order.  1 = Eastern style (last name, first name) order.
    /// </summary>
    public bool NameStyle { get; set; }

    /// <summary>
    /// A courtesy title. For example, Mr. or Ms.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// First name of the person.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Middle name or middle initial of the person.
    /// </summary>
    public string MiddleName { get; set; }

    /// <summary>
    /// Last name of the person.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Surname suffix. For example, Sr. or Jr.
    /// </summary>
    public string Suffix { get; set; }

    /// <summary>
    /// 0 = Contact does not wish to receive e-mail promotions, 1 = Contact does wish to receive e-mail promotions from AdventureWorks, 2 = Contact does wish to receive e-mail promotions from AdventureWorks and selected partners.
    /// </summary>
    public int EmailPromotion { get; set; }

    /// <summary>
    /// Additional contact information about the person stored in xml format.
    /// </summary>
    public string AdditionalContactInfo { get; set; }

    /// <summary>
    /// Personal information such as hobbies, and income collected from online shoppers. Used for sales analysis.
    /// </summary>
    public string Demographics { get; set; }

    /// <summary>
    /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
    /// </summary>
    public Guid Rowguid { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual BusinessEntity BusinessEntity { get; set; }

    public virtual ICollection<BusinessEntityContact> BusinessEntityContacts { get; } = new List<BusinessEntityContact>();

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();

    public virtual ICollection<EmailAddress> EmailAddresses { get; } = new List<EmailAddress>();

    public virtual Employee Employee { get; set; }

    public virtual Password Password { get; set; }

    public virtual ICollection<PersonCreditCard> PersonCreditCards { get; } = new List<PersonCreditCard>();

    public virtual ICollection<PersonPhone> PersonPhones { get; } = new List<PersonPhone>();

    public object[] KeyValues => new object[] { BusinessEntityId };
}