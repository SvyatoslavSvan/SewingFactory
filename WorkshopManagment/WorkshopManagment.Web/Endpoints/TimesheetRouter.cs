using Calabonga.AspNetCore.AppDefinitions;
using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Endpoints
{
    public sealed class TimesheetRouter : AppDefinition
    {
        public override void ConfigureApplication(WebApplication app)
        {
            app.MapGet("/api/Timesheet" + "/getCurrent", GetCurrent).WithTags("Timesheet");
            app.MapPost("/api/Timesheet" + "/updateWorkday",UpdateWorkday).WithTags("Timesheet");
            app.MapPost("/api/Timesheet" + "/createManualy", CreateManual).WithTags("Timesheet");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
        public async Task<OperationResult<TimesheetViewModel>> GetCurrent(
            [FromServices] IMediator mediator,
            HttpContext context)
            => await mediator.Send(new GetCurrentTimesheetRequest(context.User),
                context.RequestAborted);

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
        public async Task<OperationResult<UpdateWorkdayViewModel>> UpdateWorkday([FromBody] UpdateWorkdayViewModel model,
            [FromServices] IMediator mediator,
            HttpContext context)
            => await mediator.Send(new UpdateRequest<UpdateWorkdayViewModel,WorkDay>(model,context.User),
                context.RequestAborted);

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
        public async Task<OperationResult<TimesheetViewModel>> CreateManual(
            [FromServices] IMediator mediator,
            HttpContext context)
            => await mediator.Send(new ManualCreateTimesheetRequest(context.User),
                context.RequestAborted);

    }
}
