using Calabonga.AspNetCore.AppDefinitions;
using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.ViewModels;
using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.Routers;

public sealed class TimesheetRouter : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        var group = app.MapGroup("/api/Timesheet").WithTags("Timesheet").RequireAuthorization(AppData.FinanceAccess);
        group.MapGet("/getCurrent", GetCurrent);
        group.MapPost("/updateWorkday", UpdateWorkday);
        group.MapPost("/createManually", CreateManual);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private async Task<OperationResult<TimesheetViewModel>> GetCurrent(
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new GetCurrentTimesheetRequest(context.User),
            context.RequestAborted);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private async Task<OperationResult<UpdateWorkdayViewModel>> UpdateWorkday(
        [FromBody] UpdateWorkdayViewModel model,
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new UpdateRequest<UpdateWorkdayViewModel, WorkDay>(model, context.User),
            context.RequestAborted);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private async Task<OperationResult<TimesheetViewModel>> CreateManual(
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new ManualCreateTimesheetRequest(context.User),
            context.RequestAborted);
}