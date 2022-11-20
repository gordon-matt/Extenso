﻿namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Work shift lookup table.
/// </summary>
public partial class Shift
{
    /// <summary>
    /// Primary key for Shift records.
    /// </summary>
    public byte ShiftId { get; set; }

    /// <summary>
    /// Shift description.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Shift start time.
    /// </summary>
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Shift end time.
    /// </summary>
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<EmployeeDepartmentHistory> EmployeeDepartmentHistories { get; } = new List<EmployeeDepartmentHistory>();
}