namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Lookup table containing the departments within the Adventure Works Cycles company.
/// </summary>
public partial class Department
{
    /// <summary>
    /// Primary key for Department records.
    /// </summary>
    public short DepartmentId { get; set; }

    /// <summary>
    /// Name of the department.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Name of the group to which the department belongs.
    /// </summary>
    public string GroupName { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<EmployeeDepartmentHistory> EmployeeDepartmentHistories { get; } = new List<EmployeeDepartmentHistory>();
}