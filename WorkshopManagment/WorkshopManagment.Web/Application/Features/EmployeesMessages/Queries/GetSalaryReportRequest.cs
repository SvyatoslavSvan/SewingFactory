using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.Providers;
using SewingFactory.Common.Domain.ValueObjects;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.Queries
{
    public record GetSalaryReportRequest(
        ClaimsPrincipal User,
        DateRange Period) : IRequest<IResult>;

    public class GetSalaryReportRequestHandler(
        IUnitOfWork unitOfWork,
        IReportProvider provider) : IRequestHandler<GetSalaryReportRequest, IResult>
    {
        public async Task<IResult> Handle(
            GetSalaryReportRequest request,
            CancellationToken cancellationToken)
        {

            var employees = await unitOfWork
                .GetRepository<Employee>()
                .GetAllAsync(include: q => 
                        q.Include(x => x.Department).ThenInclude(x => x.Employees)
                        .Include(e => e.Documents
                            .Where(x => x.Date >= request.Period.Start && x.Date <= request.Period.End))
                        .ThenInclude(x => x.Tasks)
                        .ThenInclude(x => x.Process)
                        .Include(x => x.Documents
                            .Where(x => x.Date >= request.Period.Start && x.Date <= request.Period.End))
                        .ThenInclude(x => x.Tasks)
                        .ThenInclude(x => x.EmployeeTaskRepeats)
                        .ThenInclude(x => x.WorkShopEmployee)
                        .Include(e => (e as RateBasedEmployee)!.Timesheets
                            .Where(x => x.Date >= request.Period.Start && x.Date <= request.Period.End)) //TODO Create & Use method of DateRange
                        .ThenInclude(x => x.WorkDays)
                        .ThenInclude(x => x.Employee),
                    trackingType: TrackingType.NoTrackingWithIdentityResolution
                );

            var salaries = employees.Select(x => x.CalculateSalary(request.Period))
                .ToList();

            var reportBytes = await provider.GetReportAsync(salaries);


            var fileName = $"salary_report_{DateTime.UtcNow:yyyyMMdd}.xlsx";
            var fileResult = Results.File(
                reportBytes,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: fileName
            );

            return fileResult;
        }
    }
}