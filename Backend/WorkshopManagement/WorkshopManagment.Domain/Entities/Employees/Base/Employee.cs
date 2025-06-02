using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Interfaces;
using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;

/// <summary>
///     Base class for an employee in the workshop.
/// </summary>
public abstract class Employee : NamedIdentity, IHasDocuments
{
    private readonly List<WorkshopDocument> _documents;
    private Department _department = null!;
    private string _internalId = null!;

    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    protected Employee() => _documents = new List<WorkshopDocument>();

    [SetsRequiredMembers]
    protected Employee(string name, string internalId, Department department) : base(name)
    {
        InternalId = internalId;
        Department = department;
        _documents = new List<WorkshopDocument>();
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

    public IEnumerable<WorkshopDocument> Documents => _documents;

    public virtual Salary CalculateSalary(DateRange dateRange)
    {
        var documentPart = ((IHasDocuments)this).DocumentPayment(dateRange);

        return new Salary(documentPart, Money.Zero, this);
    }
}