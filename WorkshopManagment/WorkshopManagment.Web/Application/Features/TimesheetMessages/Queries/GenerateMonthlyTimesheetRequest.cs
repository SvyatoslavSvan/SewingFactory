using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Providers;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Queries;

public record GenerateMonthlyTimesheetRequest : IRequest;

public class GenerateMonthlyTimesheetRequestHandler(
    IUnitOfWork unitOfWork,
    IDateTimeProvider provider, ILogger<GenerateMonthlyTimesheetRequestHandler> logger) : IRequestHandler<GenerateMonthlyTimesheetRequest>
{
    public async Task Handle(
        GenerateMonthlyTimesheetRequest request,
        CancellationToken cancellationToken)
    {
        if (await unitOfWork.GetRepository<Timesheet>()
                .GetFirstOrDefaultAsync(predicate: x => x.Date == provider.CurrentMonthStart,
                    disableTracking: true) !=
            null)
        {
            logger.LogError($"{nameof(Timesheet)} with {provider.CurrentMonthStart} already exist");
            return;
        }

        await unitOfWork.GetRepository<Timesheet>()
            .InsertAsync(Timesheet.CreateInstance(await unitOfWork.GetRepository<RateBasedEmployee>()
                        .GetAllAsync(false),
                    provider.CurrentMonthStart),
                cancellationToken);

        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.LastSaveChangesResult.IsOk)
        {
            logger.LogCritical(unitOfWork.LastSaveChangesResult.Exception, "Cannot create a timesheet");
        }
    }
}