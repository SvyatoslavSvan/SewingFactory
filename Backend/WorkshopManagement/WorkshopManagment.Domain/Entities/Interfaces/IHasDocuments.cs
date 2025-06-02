using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Interfaces;

public interface IHasDocuments
{
    public IEnumerable<WorkshopDocument> Documents { get; }

    public Money DocumentPayment(DateRange period)
    {
        if (this is not Employee employee)
        {
            throw new InvalidOperationException("IHasDocuments Implements just Employee");
        }

        return Documents
            .Where(predicate: d => period.Contains(d.Date))
            .Aggregate(Money.Zero,
                func: (sum, doc) => sum + doc.CalculatePaymentForEmployee(employee));
    }
}