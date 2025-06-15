using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;

public sealed class Process : NamedIdentity
{
    private readonly List<GarmentModel> _garmentModels = null!;

    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private Process() { }

    public Process(string name, Department department, Money? price = null) : base(name)
    {
        Price = price ?? new Money(0);
        Department = department;
    }

    public Money Price { get; set; } = null!;

    public Department Department { get; set; } = null!;

    public List<GarmentModel> GarmentModels => _garmentModels;
}