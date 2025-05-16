using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;
using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Endpoints.Base;

public abstract class CommandRouter
<TEntity,
    TReadViewModel,
    TCreateViewModel,
    TUpdateViewModel,
    TDeleteViewModel, TDetailsReadViewModel>
    : QueryRouter<TEntity,
        TReadViewModel, TDetailsReadViewModel>
    where TEntity : Identity
    where TUpdateViewModel : IIdentityViewModel
    where TDeleteViewModel : IIdentityViewModel
{
    public override void ConfigureApplication(WebApplication app)
    {
        base.ConfigureApplication(app);
        app.MapPost(Prefix + "/create", Create).WithTags(_featureGroupName);
        app.MapPut(Prefix + "/update", Update).WithTags(_featureGroupName);
        app.MapDelete(Prefix + "/delete", Delete).WithTags(_featureGroupName);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)] //TODO: Uncomment when authentication is ready
    protected async Task<OperationResult<TDetailsReadViewModel>> Create(
        [FromBody] TCreateViewModel model,
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new CreateRequest<TCreateViewModel, TEntity, TDetailsReadViewModel>(model,
                context.User),
            context.RequestAborted);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
    protected async Task<OperationResult<TDeleteViewModel>> Delete(
        [FromBody] TDeleteViewModel model,
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new DeleteRequest<TDeleteViewModel, TEntity>(model,
                context.User),
            context.RequestAborted);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
    protected async Task<OperationResult<TUpdateViewModel>> Update(
        [FromBody] TUpdateViewModel model,
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new UpdateRequest<TUpdateViewModel, TEntity>(model,
                context.User),
            context.RequestAborted);
}