using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;

public class Salary(Money payment, Money premium, ProcessBasedEmployee employee)
{
    public Money Payment { get; init; } = payment;

    public Money Premium { get; init; } = premium;

    public ProcessBasedEmployee Employee { get; set; } = employee ?? throw new SewingFactoryArgumentNullException(nameof(Employee));

    public Money TakeHome => Payment + Premium;
}