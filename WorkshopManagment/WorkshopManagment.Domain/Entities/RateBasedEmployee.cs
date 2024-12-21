using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities
{
    public sealed class RateBasedEmployee(string name, string internalId, Money rate,Department department, List<WorkshopDocument> documents, int premium = 0)
        : ProcessBasedEmployee(name, internalId,department, documents ,premium)
    {
        private readonly List<Timesheet> _timesheets = [];

        public Money Rate { get; set; } = rate;

        public IReadOnlyList<Timesheet> Timesheets => _timesheets;

        public override Salary CalculateSalary(DateRange dateRange)
        {
            var documentsPayment = base.CalculateSalary(dateRange).Payment;
            var timesheets = _timesheets.Where(x => dateRange.Contains(x.Date));
            var ratePayment = new Money(0);
            ratePayment = timesheets.Aggregate(ratePayment, (current, timesheet) => current + timesheet.HoursWorked(this) * new Money(Rate.Amount / timesheet.Hours));
            return new Salary(documentsPayment + ratePayment, (documentsPayment + ratePayment) * Premium, this);
        }
    }
}
