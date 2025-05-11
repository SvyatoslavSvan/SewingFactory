using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;

public class Salary
{
    public Salary(Money payment,
        Money premium,
        ProcessBasedEmployee employee)
        : this(payment, premium, employee, new Money(0)) { }

    public Salary(Money payment,
        Money premium,
        ProcessBasedEmployee employee,
        Money additionalPayment)
    {
        Payment = payment;
        Premium = premium;
        Employee = employee ?? throw new SewingFactoryArgumentNullException(nameof(employee));
        AdditionalPayment = additionalPayment;
    }


    public Money Payment { get; init; }

    public Money Premium { get; init; }

    public ProcessBasedEmployee Employee { get; set; }

    public Money TakeHome => Payment + Premium;

    public Money AdditionalPayment { get; init; }
}