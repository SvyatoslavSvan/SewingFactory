using MediatR;
using Quartz;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Queries;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.QuartzJobs;

public class GenerateMonthlyTimesheetJob(
    IMediator mediator) : IJob
{
    public async Task Execute(
        IJobExecutionContext context)
        => await mediator.Send(new GenerateMonthlyTimesheetRequest(),
            CancellationToken.None);
}