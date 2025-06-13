using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Routers;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Routers;

public sealed class GarmentModelRouter
    : CommandRouter<GarmentModel,
        ReadGarmentModelViewModel,
        CreateGarmentModelViewModel,
        UpdateGarmentModelViewModel,
        DeleteGarmentModelViewModel, DetailsReadGarmentModelViewModel>
{
    public override void ConfigureApplication(WebApplication app)
    {
        base.ConfigureApplication(app);
        app.MapGet(Prefix + "/getForCreate", GetForCreate).WithTags(_featureGroupName);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
    public async Task<OperationResult<GetForCreateGarmentModelViewModel>> GetForCreate(
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new GetForCreateGarmentModelRequest(context.User),
            context.RequestAborted);
}