using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.Providers;
using SewingFactory.Common.Domain.ValueObjects;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.Queries;

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
                    q.Include(navigationPropertyPath: x => x.Department).ThenInclude(navigationPropertyPath: x => x.Employees)
                        .Include(navigationPropertyPath: e => e.Documents
                            .Where(x => x.Date >= request.Period.Start && x.Date <= request.Period.End))
                        .ThenInclude(navigationPropertyPath: x => x.Tasks)
                        .ThenInclude(navigationPropertyPath: x => x.Process)
                        .Include(navigationPropertyPath: x => x.Documents
                            .Where(x => x.Date >= request.Period.Start && x.Date <= request.Period.End))
                        .ThenInclude(navigationPropertyPath: x => x.Tasks)
                        .ThenInclude(navigationPropertyPath: x => x.EmployeeTaskRepeats)
                        .ThenInclude(navigationPropertyPath: x => x.WorkShopEmployee)
                        .Include(navigationPropertyPath: e => (e as RateBasedEmployee)!.Timesheets
                            .Where(d =>
                                (d.Date.Year > request.Period.Start.Year ||
                                 (d.Date.Year == request.Period.Start.Year && d.Date.Month >= request.Period.Start.Month))
                                &&
                                (d.Date.Year < request.Period.End.Year ||
                                 (d.Date.Year == request.Period.End.Year && d.Date.Month <= request.Period.End.Month))
                            )) //TODO Create & Use method of DateRange
                        .ThenInclude(navigationPropertyPath: x => x.WorkDays)
                        .ThenInclude(navigationPropertyPath: x => x.Employee),
                trackingType: TrackingType.NoTrackingWithIdentityResolution
            );

        var salaries = employees.Select(selector: x => x.CalculateSalary(request.Period))
            .ToList();

        var reportBytes = await provider.GetReportAsync(salaries);

        var fileName = $"salary_report_{DateTime.UtcNow:yyyyMMdd}.xlsx";
        var fileResult = Results.File(
            reportBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName
        );

        return fileResult;
    }
}