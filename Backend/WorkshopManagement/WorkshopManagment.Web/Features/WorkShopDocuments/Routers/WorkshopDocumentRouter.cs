using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Routers;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;
using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.Routers;

public sealed class WorkshopDocumentRouter : CommandRouter<
    WorkshopDocument,
    ReadWorkshopDocumentViewModel,
    CreateWorkshopDocumentViewModel,
    UpdateWorkshopDocumentViewModel,
    DeleteWorkshopDocumentViewModel,
    DetailsReadWorkshopDocumentViewModel
>
{
    protected override string PolicyName => AppData.FinanceAccess;

    public override void ConfigureApplication(WebApplication app)
    {
        base.ConfigureApplication(app);
        _group?.MapGet("/getForCreate", GetForCreate);
        _group?.MapGet("/getForUpdate", GetForUpdate);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private async Task<OperationResult<GetForCreateWorkshopDocumentViewModel>> GetForCreate(
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new GetForCreateWorkshopDocumentRequest(context.User),
            context.RequestAborted);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private async Task<OperationResult<GetForUpdateWorkshopDocumentViewModel>> GetForUpdate(
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new GetForUpdateWorkshopDocumentRequest(context.User),
            context.RequestAborted);
}