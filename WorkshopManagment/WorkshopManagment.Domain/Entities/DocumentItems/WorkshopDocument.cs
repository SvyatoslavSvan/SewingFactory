using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;

public sealed class WorkshopDocument : Identity
{
    private readonly List<ProcessBasedEmployee> _employees;
    private readonly List<WorkshopTask> _tasks;
    private int _countOfModelsInvolved;
    private GarmentModel _garmentModel = null!;
    private Department _department = null!;


    /// <summary>
    ///     default constructor for EF Core
    /// </summary>
    private WorkshopDocument()
    {
        _tasks = [];
        _employees = [];
    }

    private WorkshopDocument(int countOfModelsInvolved, GarmentModel garmentModel, List<WorkshopTask> tasks)
    {
        CountOfModelsInvolved = countOfModelsInvolved;
        GarmentModel = garmentModel;
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
        set => _department = value ?? throw new SewingFactoryArgumentNullException(nameof(value));
    }

    public IReadOnlyList<WorkshopTask> Tasks => _tasks;

    public IReadOnlyList<ProcessBasedEmployee> Employees => _employees;

    public static WorkshopDocument CreateInstance(
        int countOfModelsInvolved,
        GarmentModel garmentModel,
        Department department)
        => new(countOfModelsInvolved,
            garmentModel,
            [
                ..garmentModel.Processes.Where(x => x.Department == department)
                    .Select(selector: process => new WorkshopTask(process))
            ]);

    public Money CalculatePaymentForEmployee(ProcessBasedEmployee employee)
    {
        var payment = new Money(0);
        var tasksForEmployee = _tasks.Where(predicate: task => task.EmployeesInvolved.Contains(employee)).ToList();
        payment = tasksForEmployee.Aggregate(payment, func: (current, workshopTask) => current + workshopTask.CalculatePaymentForEmployee(employee));

        return payment;
    }
}