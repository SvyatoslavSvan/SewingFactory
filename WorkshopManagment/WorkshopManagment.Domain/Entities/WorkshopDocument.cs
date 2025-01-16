using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities
{
    public sealed class WorkshopDocument : Identity
    {
        private int _countOfModelsInvolved;
        private GarmentModel _garmentModel = null!;
        private readonly List<WorkshopTask> _tasks;
        private readonly List<ProcessBasedEmployee> _employees;
        private static readonly Dictionary<Department, Func<GarmentModel, IEnumerable<WorkshopTask>>> DepartmentTaskMapping =
            new()
            {
                { Department.Sewing, model => model.SewingProcesses.Select(process => new WorkshopTask(process)) },
                { Department.Cutting, model => model.CuttingProcesses.Select(process => new WorkshopTask(process)) },
                { Department.Pressing, model => model.PressingProcesses.Select(process => new WorkshopTask(process)) }
            };

        private WorkshopDocument(int countOfModelsInvolved, GarmentModel garmentModel, List<WorkshopTask> tasks)
        {
            CountOfModelsInvolved = countOfModelsInvolved;
            GarmentModel = garmentModel;
            _tasks = tasks;
            _employees = tasks.SelectMany(task => task.EmployeeTaskRepeats).Select(repeat => repeat.WorkShopEmployee).Distinct().ToList();
        }

        public static WorkshopDocument CreateInstance(int countOfModelsInvolved, GarmentModel garmentModel, Department department)
        {
            var tasks = new List<WorkshopTask>();
            if (DepartmentTaskMapping.TryGetValue(department, out var taskGenerator))
            {
                tasks.AddRange(taskGenerator(garmentModel));
            }
            else
            {
                throw new SewingFactoryArgumentException("Unknown department type.", nameof(department));
            }
            return new WorkshopDocument(countOfModelsInvolved, garmentModel, tasks);
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

        public Department Department { get; init; }

        public IReadOnlyList<WorkshopTask> Tasks => _tasks;

        public IReadOnlyList<ProcessBasedEmployee> Employees => _employees;

        public Money CalculatePaymentForEmployee(ProcessBasedEmployee employee)
        {
            var payment = new Money(0);
            var tasksForEmployee = _tasks.Where(task => task.EmployeesInvolved.Contains(employee)).ToList();
            payment = tasksForEmployee.Aggregate(payment, (current, workshopTask) => current + workshopTask.CalculatePaymentForEmployee(employee));
            return payment;
        }
    }
}
