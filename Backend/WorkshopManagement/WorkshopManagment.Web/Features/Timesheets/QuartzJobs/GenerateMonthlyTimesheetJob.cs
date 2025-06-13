using MediatR;
using Quartz;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.Queries;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.QuartzJobs;

public class GenerateMonthlyTimesheetJob(
    IMediator mediator) : IJob
{
    public async Task Execute(
        IJobExecutionContext context)
        => await mediator.Send(new GenerateMonthlyTimesheetRequest(),
            CancellationToken.None);
}