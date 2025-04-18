﻿using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;

public class SalaryReport
{
    private readonly List<Salary> _salaries = null!;

    public SalaryReport(List<Salary> salaries) => Salaries = salaries;

    public IReadOnlyList<Salary> Salaries
    {
        get => _salaries;
        init
        {
            if (Salaries.Count == 0)
            {
                throw new SewingFactoryArgumentException(nameof(Salaries), "Cannot create salary report without salaries");
            }

            _salaries = value.ToList();
        }
    }
}