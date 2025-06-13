using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.Providers;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.Queries;

public record GenerateMonthlyTimesheetRequest : IRequest;

public class GenerateMonthlyTimesheetRequestHandler(
    IUnitOfWork unitOfWork,
    IDateTimeProvider provider,
    ILogger<GenerateMonthlyTimesheetRequestHandler> logger) : IRequestHandler<GenerateMonthlyTimesheetRequest>
{
    public async Task Handle(
        GenerateMonthlyTimesheetRequest request,
        CancellationToken cancellationToken)
    {
        if (await unitOfWork.GetRepository<Timesheet>()
                .GetFirstOrDefaultAsync(predicate: x => x.Date == provider.CurrentMonthStart,
                    trackingType: TrackingType.NoTracking) !=
            null)
        {
            logger.LogError($"{nameof(Timesheet)} with {provider.CurrentMonthStart} already exist");

            return;
        }

        await unitOfWork.GetRepository<Timesheet>()
            .InsertAsync(Timesheet.CreateInstance(await unitOfWork.GetRepository<RateBasedEmployee>()
                        .GetAllAsync(TrackingType.Tracking),
                    provider.CurrentMonthStart),
                cancellationToken);

        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.Result.Ok)
        {
            logger.LogCritical(unitOfWork.Result.Exception, "Cannot create a timesheet");
        }
    }
}