using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Providers;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Queries;

public record GetCurrentTimesheetRequest(
    ClaimsPrincipal User) : IRequest<OperationResult<TimesheetViewModel>>;

public class GetCurrentTimesheetRequestHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper,
    IDateTimeProvider provider) : IRequestHandler<GetCurrentTimesheetRequest, OperationResult<TimesheetViewModel>>
{
    public async Task<OperationResult<TimesheetViewModel>> Handle(
        GetCurrentTimesheetRequest request,
        CancellationToken cancellationToken)
    {
        var operationResult = OperationResult.CreateResult<TimesheetViewModel>();
        var timesheet = await unitOfWork.GetRepository<Timesheet>()
            .GetFirstOrDefaultAsync(predicate: x => x.Date == provider.CurrentMonthStart,
                trackingType: TrackingType.Tracking,
                include: x => x.Include(navigationPropertyPath: t => t.WorkDays)
                    .ThenInclude(navigationPropertyPath: w => w.Employee));

        if (timesheet is null)
        {
            operationResult.AddError(new SewingFactoryNotFoundException($"Timesheet with date {provider.CurrentMonthStart.ToString()} doesn't exist"));

            return operationResult;
        }

        operationResult.Result = mapper.Map<TimesheetViewModel>(timesheet,
            opts: opts => opts.Items["Timesheet"] = timesheet);

        return operationResult;
    }
}