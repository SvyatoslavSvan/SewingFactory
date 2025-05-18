using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;

public sealed class EmployeeTaskRepeat : Identity
{
    private int _repeats;
    private Employee _workShopEmployee = null!;
    private WorkshopTask _workshopTask = null!;

    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private EmployeeTaskRepeat() { }

    public EmployeeTaskRepeat(ProcessBasedEmployee workShopEmployee, int repeats)
    {
        WorkShopEmployee = workShopEmployee;
        Repeats = repeats;
    }

    public Employee WorkShopEmployee
    {
        get => _workShopEmployee;
        set => _workShopEmployee = value ?? throw new SewingFactoryArgumentNullException(nameof(WorkShopEmployee));
    }

    public WorkshopTask WorkshopTask
    {
        get => _workshopTask;
        set => _workshopTask = value ?? throw new SewingFactoryArgumentNullException(nameof(WorkshopTask));
    }

    public int Repeats
    {
        get => _repeats;
        set
        {
            if (value < 0)
            {
                throw new SewingFactoryArgumentException(nameof(Repeats), "Repeats cannot be negative");
            }

            _repeats = value;
        }
    }
}