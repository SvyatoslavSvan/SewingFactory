using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Base;

/// <summary>
///     Base class for an employee in the workshop.
/// </summary>
public abstract class Employee : NamedIdentity
{
    private string _internalId = null!;
    private Department _department = null!;

    /// <summary>
    /// Default constructor for EF Core
    /// </summary>
    protected Employee()
    { }

    [SetsRequiredMembers]
    protected Employee(string name, string internalId, Department department) : base(name)
    {
        InternalId = internalId;
        Department = department;
    }

    /// <summary>
    ///     Gets or sets the internal identifier for the employee.
    /// </summary>
    /// <exception cref="SewingFactoryArgumentNullException">Thrown when the value is null, empty, or whitespace.</exception>
    public required string InternalId
    {
        get => _internalId;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new SewingFactoryArgumentNullException(nameof(value));
            }

            _internalId = value;
        }
    }

    public Department Department
    {
        get => _department;
        set => _department = value ?? throw new SewingFactoryArgumentNullException(nameof(value));
    }

    public abstract Salary CalculateSalary(DateRange dateRange);
}