using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.Interfaces;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;

public sealed class GarmentModel : NamedIdentity, IPrototype<GarmentModel>
{
    private readonly List<Process> _cuttingProcesses = null!;
    private readonly List<Process> _pressingProcesses = null!;
    private readonly List<Process> _sewingProcesses = null!;
    private GarmentCategory _category = null!;
    private string _description = null!;

    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private GarmentModel() { }

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

    public GarmentModel Clone() => throw new NotImplementedException();

    public void AddProcess(Process process)
    {
        if (process == null)
        {
            throw new SewingFactoryArgumentNullException(nameof(process));
        }

        _sewingProcesses.Add(process);
    }

    public void RemoveProcess(Process process) => _sewingProcesses.Remove(SewingProcesses.FirstOrDefault(predicate: x => x.Id == process.Id)!);
}