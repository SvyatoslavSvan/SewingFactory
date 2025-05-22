using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.Providers
{
    public interface IReportProvider
    {
        public Task<byte[]> GetReportAsync(IList<Salary> salaries);
    }
}
