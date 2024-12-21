using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities;

public class WorkshopTask : Identity
{
    private readonly List<EmployeeTaskRepeat> _employeeTaskRepeats;
    private Process _process = null!;
    private WorkshopDocument _document = null!;

    public WorkshopTask(Process process)
    {
        _employeeTaskRepeats = [];
        Process = process;
    }

    public WorkshopTask(Process process, List<EmployeeTaskRepeat> employeeTaskRepeats, WorkshopDocument document)
    {
        Process = process;
        _employeeTaskRepeats = employeeTaskRepeats;
        Document = document;
    }

    public Process Process
    {
        get => _process;
        set => _process = value ?? throw new BizSuiteArgumentNullException(nameof(Process));
    }

    public WorkshopDocument Document
    {
        get => _document;
        set => _document = value ?? throw new BizSuiteArgumentNullException(nameof(Document));
    }

    public IReadOnlyList<EmployeeTaskRepeat> EmployeeTaskRepeats => _employeeTaskRepeats;

    public List<ProcessBasedEmployee> EmployeesInvolved => _employeeTaskRepeats.Select(x => x.WorkShopEmployee).ToList();

    public void AddEmployeeRepeat(EmployeeTaskRepeat employeeRepeat)
    {
        if (employeeRepeat == null)
        {
            throw new BizSuiteArgumentNullException(nameof(employeeRepeat));
        }

        if (_employeeTaskRepeats.FirstOrDefault(x => x.WorkShopEmployee.Id == employeeRepeat.WorkShopEmployee.Id) != null)
        {
            throw new BizSuiteArgumentException(nameof(employeeRepeat),
                $"The employee with ID '{employeeRepeat.WorkShopEmployee.Id}' is already assigned to this task. Duplicate assignments are not allowed");
        }

        _employeeTaskRepeats.Add(employeeRepeat);
    }

    public Money CalculatePaymentForEmployee(ProcessBasedEmployee employee)
    {
        var employeeTaskRepeat = _employeeTaskRepeats.FirstOrDefault(x => x.WorkShopEmployee.Id == employee.Id);
        if (employeeTaskRepeat == null)
        {
            throw new BizSuiteInvalidOperationException("Cannot calculate payment for not existing in task employee");
        }
        return new Money(employeeTaskRepeat.Repeats * _process.Price.Amount);
    }
}