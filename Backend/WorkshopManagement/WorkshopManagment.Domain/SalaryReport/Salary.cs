using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;

public sealed class Salary(
    Money payment,
    Money premium,
    Employee employee,
    Money additionalPayment)
{
    public Salary(
        Money payment,
        Money premium,
        Employee employee)
        : this(payment, premium, employee, new Money(0))
    {
    }


    public Money Payment { get; set; } = payment;

    public Money Premium { get; set; } = premium;

    public Employee Employee { get; set; } = employee ?? throw new SewingFactoryArgumentNullException(nameof(employee));

    public Money TakeHome => Payment + Premium + AdditionalPayment;

    public Money AdditionalPayment { get; set; } = additionalPayment;
}