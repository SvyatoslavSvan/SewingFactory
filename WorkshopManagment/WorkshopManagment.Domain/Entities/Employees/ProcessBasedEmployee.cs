using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;

public class ProcessBasedEmployee : Employee
{
    private List<WorkshopDocument>? _documents;

    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    protected ProcessBasedEmployee() => Premium = 0;

    [SetsRequiredMembers]
    public ProcessBasedEmployee(string name, string internalId, Department department) : base(name, internalId, department) => Premium = new Percent(0);

    [SetsRequiredMembers]
    public ProcessBasedEmployee(string name, string internalId, Department department, Percent premium) : base(name, internalId, department) => Premium = premium;

    public IEnumerable<WorkshopDocument>? Documents
    {
        get => _documents;
        set => _documents = value?.ToList();
    }

    public Percent Premium { get; set; }

    public override Salary CalculateSalary(DateRange dateRange)
    {
        if (_documents == null)
        {
            throw new SewingFactoryArgumentException(nameof(Documents), "Cannot calculate salary without documents");
        }

        var documents = _documents.Where(predicate: document => dateRange.Contains(document.Date)); //TODO: contains
        var documentsPayment = new Money(0);
        documentsPayment = documents.Aggregate(documentsPayment, func: (current, workshopDocument) => current + workshopDocument.CalculatePaymentForEmployee(this));

        return new Salary(documentsPayment, documentsPayment * Premium, this);
    }
}