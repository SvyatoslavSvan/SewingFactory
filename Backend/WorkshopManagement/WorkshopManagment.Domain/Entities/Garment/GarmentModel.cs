﻿using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.Interfaces;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;

public sealed class GarmentModel : NamedIdentity, IPrototype<GarmentModel>
{
    private readonly List<Process> _processes = [];
    private GarmentCategory _category = null!;
    private string _description = null!;

    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private GarmentModel() => Price = Money.Zero;

    public GarmentModel(
        string name,
        string description,
        List<Process> processes,
        GarmentCategory category,
        Money price,
        byte[]? image = null) : base(name)
    {
        Description = description;
        _processes = processes;
        Category = category;
        Price = price;
        Image = image;
    }

    public GarmentModel(
        string name,
        string description,
        List<Process> processes,
        GarmentCategory category,
        Money price,
        Guid id,
        byte[]? image = null) : base(name, id)
    {
        Description = description;
        _processes = processes;
        Category = category;
        Price = price;
        Image = image;
    }

    public IReadOnlyList<Process> Processes => _processes;

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

    public Money Price { get; set; }

    public byte[]? Image { get; set; }

    public GarmentModel Clone() => new(Name, Description, _processes.ToList(), Category, Price, Image);

    public void ReplaceProcesses(IEnumerable<Process> processes)
    {
        _processes.Clear();
        _processes.AddRange(processes);
    }
}