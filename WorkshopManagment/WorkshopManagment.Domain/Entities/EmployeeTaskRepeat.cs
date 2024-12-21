using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities;

public class EmployeeTaskRepeat : Identity
{
    private int _repeats;
    private ProcessBasedEmployee _workShopEmployee = null!;
    private WorkshopTask _workshopTask = null!;

    public EmployeeTaskRepeat(ProcessBasedEmployee workShopEmployee, int repeats)
    {
        WorkShopEmployee = workShopEmployee;
        Repeats = repeats;
    }

    public ProcessBasedEmployee WorkShopEmployee
    {
        get => _workShopEmployee;
        set => _workShopEmployee = value ?? throw new BizSuiteArgumentNullException(nameof(WorkShopEmployee));
    }

    public WorkshopTask WorkshopTask
    {
        get => _workshopTask;
        set => _workshopTask = value ?? throw new BizSuiteArgumentNullException(nameof(WorkshopTask));
    }

    public int Repeats
    {
        get => _repeats;
        set
        {
            if (value < 0)
            {
                throw new BizSuiteArgumentException(nameof(Repeats), "Repeats cannot be negative");
            }
            _repeats = value;
        }
    }
}