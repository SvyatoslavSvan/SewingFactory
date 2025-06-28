using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Routers;

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
        _group?.MapPost("/create", Create);
        _group?.MapPut("/update", Update);
        _group?.MapDelete("/delete", Delete);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    protected async Task<OperationResult<TUpdateViewModel>> Update(
        [FromBody] TUpdateViewModel model,
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new UpdateRequest<TUpdateViewModel, TEntity>(model,
                context.User),
            context.RequestAborted);
}