using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;

public sealed class WorkshopDocument : NamedIdentity
{
    private readonly Department _department = null!;
    private readonly List<Employee> _employees;
    private readonly List<WorkshopTask> _tasks;
    private int _countOfModelsInvolved;
    private GarmentModel _garmentModel = null!;


    /// <summary>
    ///     default constructor for EF Core
    /// </summary>
    private WorkshopDocument()
    {
        _employees = [];
        _tasks = [];
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

    public IReadOnlyList<Employee> Employees => _employees;

    public void RecalculateEmployees()
    {
        var newEmployees = _tasks
            .SelectMany(selector: t => t.EmployeesInvolved)
            .DistinctBy(keySelector: e => e.Id)
            .ToList();

        var toRemove = _employees
            .Where(predicate: old => newEmployees.All(predicate: ne => ne.Id != old.Id))
            .ToList();

        foreach (var old in toRemove)
        {
            _employees.Remove(old);
        }

        foreach (var emp in newEmployees.Where(emp => _employees.All(predicate: e => e.Id != emp.Id)))
        {
            _employees.Add(emp);
        }
    }
    
    public void UpdateTaskRepeats(IEnumerable<TaskRepeatInfo> updates)
    {
        foreach (var info in updates)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == info.TaskId)
                       ?? throw new SewingFactoryNotFoundException(
                           $"Task {info.TaskId} not found in document {Id}");
            
            task.ReplaceRepeats(info.Repeats);
        }
        RecalculateEmployees();
    }

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

    public Money CalculatePaymentForEmployee(Employee employee)
    {
        var employeeTasks = _tasks
            .Where(predicate: t => t.EmployeesInvolved.Contains(employee));

        var total = employeeTasks.Aggregate(
            Money.Zero,
            func: (sum, task) => sum + task.CalculatePaymentForEmployee(employee));

        return total;
    }
}