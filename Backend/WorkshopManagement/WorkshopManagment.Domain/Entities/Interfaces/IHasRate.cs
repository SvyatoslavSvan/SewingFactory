using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Interfaces;

public interface IHasRate
{
    public Money Rate { get; set; }

    public IReadOnlyList<Timesheet> Timesheets { get; }

    public Money RatePayment(DateRange period)
    {
        if (this is not Employee)
        {
            throw new SewingFactoryInvalidOperationException("IHasRate can be implemented only in employee type");
        }

        var timesheets = Timesheets.Where(predicate: x => period.Contains(x.Date));
        var ratePayment = new Money(0);
        ratePayment = timesheets.Aggregate(ratePayment, func: (current, timesheet) => current + (timesheet.HoursWorked(this) * new Money(Rate.Amount / timesheet.Hours)));

        return ratePayment;
    }
}