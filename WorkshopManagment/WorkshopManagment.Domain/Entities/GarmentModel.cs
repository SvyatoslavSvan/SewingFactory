using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.Interfaces;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities
{
    public sealed class GarmentModel : NamedIdentity, IPrototype<GarmentModel>
    {
        private string _description = null!;
        private readonly List<Process> _cuttingProcesses;
        private readonly List<Process> _sewingProcesses;
        private readonly List<Process> _pressingProcesses;
        private GarmentCategory _category = null!;

        public GarmentModel(
            string name,
            List<Process> sewingProcesses,
            string description,
            List<Process> cuttingProcesses,
            List<Process> pressingProcesses,
            GarmentCategory category,
            byte[]? image = null) : base(name)
        {
            _sewingProcesses = sewingProcesses;
            Description = description;
            _cuttingProcesses = cuttingProcesses;
            _pressingProcesses = pressingProcesses;
            Image = image ?? [];
            Category = category;
        }

        public IReadOnlyList<Process> SewingProcesses => _sewingProcesses.AsReadOnly();

        public IReadOnlyList<Process> CuttingProcesses => _cuttingProcesses;

        public IReadOnlyList<Process> PressingProcesses => _pressingProcesses;


        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new SewingFactoryArgumentNullException(nameof(Description));
                }
                _description = value;
            }
        }

        public GarmentCategory Category
        {
            get => _category;
            set => _category = value ?? throw new SewingFactoryArgumentNullException(nameof(Category));
        }

        public byte[]? Image { get; set; }

        public void AddProcess(Process process)
        {
            if (process == null)
            {
                throw new SewingFactoryArgumentNullException(nameof(process));
            }
            _sewingProcesses.Add(process);
        }

        public void RemoveProcess(Process process) => _sewingProcesses.Remove(SewingProcesses.FirstOrDefault(x => x.Id == process.Id)!);

        public GarmentModel Clone() => throw new NotImplementedException();
    }
}
