using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Providers;

public interface IReportProvider
{
    public Task<byte[]> GetReportAsync(IList<Salary> salaries);
}