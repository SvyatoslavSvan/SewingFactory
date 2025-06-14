﻿using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;

public sealed class Department : NamedIdentity
{
    private readonly List<WorkshopDocument> _documents = null!;
    private readonly List<Employee> _employees = [];
    private readonly List<Process> _processes = null!;

    /// <summary>
    ///     Empty ctor for ef core
    /// </summary>
    private Department()
    {
    }

    public Department(string name) : base(name)
    {
    }

    public IEnumerable<Process> Processes => _processes;

    public IEnumerable<Employee> Employees => _employees;

    public IEnumerable<WorkshopDocument> Documents => _documents;

    public void AddEmployee(Employee employee) => _employees.Add(employee);
}