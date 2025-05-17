using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Document;
using SewingFactory.Backend.WorkshopManagement.Web.Endpoints.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Endpoints;

public class WorkshopDocumentRouter : CommandRouter<
    WorkshopDocument,
    ReadWorkshopDocumentViewModel,
    CreateWorkshopDocumentViewModel,
    UpdateWorkshopDocumentViewModel,
    DeleteWorkshopDocumentViewModel,
    DetailsReadWorkshopDocumentViewModel
>
{
    public override void ConfigureApplication(WebApplication app)
    {
        base.ConfigureApplication(app);
        app.MapGet(Prefix + "/getForCreate", GetForCreate).WithTags(_featureGroupName);
        app.MapGet(Prefix + "/getForUpdate", GetForUpdate).WithTags(_featureGroupName);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
    public async Task<OperationResult<GetForCreateWorkshopDocumentViewModel>> GetForCreate(
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new GetForCreateWorkshopDocumentRequest(context.User),
            context.RequestAborted);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
    public async Task<OperationResult<GetForUpdateWorkshopDocumentViewModel>> GetForUpdate(
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new GetForUpdateWorkshopDocumentRequest(context.User),
            context.RequestAborted);
}