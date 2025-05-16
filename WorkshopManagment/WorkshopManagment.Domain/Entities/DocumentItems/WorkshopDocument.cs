using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;

public sealed class WorkshopDocument : NamedIdentity
{
    private readonly List<ProcessBasedEmployee> _employees;
    private readonly List<WorkshopTask> _tasks;
    private int _countOfModelsInvolved;
    private GarmentModel _garmentModel = null!;
    private readonly Department _department = null!;


    /// <summary>
    ///     default constructor for EF Core
    /// </summary>
    private WorkshopDocument()
    {
        _tasks = [];
        _employees = [];
    }

    private WorkshopDocument(string name, int countOfModelsInvolved, DateOnly date, GarmentModel garmentModel, Department department, List<WorkshopTask> tasks)
    {
        CountOfModelsInvolved = countOfModelsInvolved;
        GarmentModel = garmentModel;
        Name = name;
        Department = department;
        Date = date;
        _tasks = tasks;
        _employees = tasks.SelectMany(selector: task => task.EmployeeTaskRepeats).Select(selector: repeat => repeat.WorkShopEmployee).Distinct().ToList();
    }

    private void RecalculateEmployees(List<Guid> existingIds)
    {
        _employees.Clear();
        _employees.AddRange(
            _tasks
                .SelectMany(t => t.EmployeeTaskRepeats)
                .Select(r => r.WorkShopEmployee)
                .Where(e => !existingIds.Contains(e.Id))   
                .DistinctBy(e => e.Id)                     
        );
    }

    public int CountOfModelsInvolved
    {
        get => _countOfModelsInvolved;

        set
        {
            if (value < 0)
            {
                throw new SewingFactoryArgumentException(nameof(CountOfModelsInvolved), "Field CountOfModelsInvolved cannot be negative");
            }

            _countOfModelsInvolved = value;
        }
    }

    public GarmentModel GarmentModel
    {
        get => _garmentModel;

        set => _garmentModel = value ?? throw new SewingFactoryArgumentNullException(nameof(GarmentModel));
    }

    public DateOnly Date { get; set; }

    public Department Department
    {
        get => _department;
        init => _department = value ?? throw new SewingFactoryArgumentNullException(nameof(value));
    }

    public IReadOnlyList<WorkshopTask> Tasks => _tasks;

    public IReadOnlyList<ProcessBasedEmployee> Employees => _employees;

    public static WorkshopDocument CreateInstance(
        string name,
        int countOfModelsInvolved, DateOnly date,
        GarmentModel garmentModel,
        Department department)
        => new(name, countOfModelsInvolved, date,
            garmentModel, department,
            [
                ..garmentModel.Processes.Where(predicate: x => x.Department.Id == department.Id)
                    .Select(selector: process => new WorkshopTask(process))
            ]);

    public Money CalculatePaymentForEmployee(ProcessBasedEmployee employee)
    {
        var payment = new Money(0);
        var tasksForEmployee = _tasks.Where(predicate: task => task.EmployeesInvolved.Contains(employee)).ToList();
        payment = tasksForEmployee.Aggregate(payment, func: (current, workshopTask) => current + workshopTask.CalculatePaymentForEmployee(employee));

        return payment;
    }

    public void ApplyUpdatedTasks(List<WorkshopTask> updatedTasks, List<Guid> existingIds)
    {
        updatedTasks
            .Join(
                _tasks,
                updated => updated.Id,
                existing => existing.Id,
                (updated, existing) => new { updated, existing }
            )
            .SelectMany(
                pair => pair.updated.EmployeeTaskRepeats,
                (pair, uRepeat) => new { existingTask = pair.existing, updatedRepeat = uRepeat }
            )
            .ToList()
            .ForEach(x =>
            x.existingTask.AddOrUpdateEmployeeRepeat(x.updatedRepeat));
        RecalculateEmployees(existingIds);
    }
}