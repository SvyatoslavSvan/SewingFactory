using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Interfaces;

public interface IHasSalaryPercentage
{
    public Percent SalaryPercentage { get; set; }

    public Money SalaryPercentagePayment(DateRange period)
    {
        if (this is not Employee employee)
        {
            throw new SewingFactoryInvalidOperationException("IHasSalaryPercentage can be implemented only in employee type");
        }

        var deptTotal = employee.Department.Employees.Where(predicate: x => x.Id != employee.Id)
            .Sum(selector: e => e.CalculateSalary(period).Payment.Amount);

        return new Money(deptTotal * SalaryPercentage.ToDecimal());
    }
}