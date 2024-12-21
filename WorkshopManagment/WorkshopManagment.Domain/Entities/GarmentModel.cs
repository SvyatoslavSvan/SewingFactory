using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.Interfaces;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities
{
    public sealed class GarmentModel : NamedIdentity, IPrototype<GarmentModel>
    {
        private string _number = null!;
        private string _description = null!;
        private readonly List<Process> _cuttingProcesses;
        private readonly List<Process> _sewingProcesses;
        private readonly List<Process> _pressingProcesses;

        public GarmentModel(string name, List<Process> sewingProcesses, string number, string description, List<Process> cuttingProcesses, List<Process> pressingProcesses, byte[]? image = null) : base(name)
        {
            _sewingProcesses = sewingProcesses;
            Number = number;
            Description = description;
            _cuttingProcesses = cuttingProcesses;
            _pressingProcesses = pressingProcesses;
            Image = image ?? [];
        }

        public IReadOnlyList<Process> SewingProcesses => _sewingProcesses.AsReadOnly();

        public IReadOnlyList<Process> CuttingProcesses => _cuttingProcesses;

        public IReadOnlyList<Process> PressingProcesses => _pressingProcesses;

        public string Number
        {
            get => _number;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new BizSuiteArgumentNullException(nameof(Number));
                }
                _number = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new BizSuiteArgumentNullException(nameof(Number));
                }
                _description = value;
            }
        }

        public byte[]? Image { get; set; }


        public void AddProcess(Process process)
        {
            if (process == null)
            {
                throw new BizSuiteArgumentNullException(nameof(process));
            }
            _sewingProcesses.Add(process);
        }

        public void RemoveProcess(Process process) => _sewingProcesses.Remove(SewingProcesses.FirstOrDefault(x => x.Id == process.Id)!);

        public GarmentModel Clone() => throw new NotImplementedException();
    }
}
