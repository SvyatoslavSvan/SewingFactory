using Calabonga.AspNetCore.AppDefinitions;
using Calabonga.OperationResults;
using Calabonga.PagedListCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Endpoints.Base;

public abstract class QueryRouter<TEntity, TReadViewModel, TDetailsReadViewModel> : AppDefinition where TEntity : Identity
{
    protected static readonly string _featureGroupName = typeof(TEntity).Name;
    protected string Prefix => "/api/" + _featureGroupName;

    public override void ConfigureApplication(WebApplication app)
    {
        app.MapGet(Prefix + "/getAll", GetAll).WithTags(_featureGroupName);
        app.MapGet(Prefix + "/getPaged/{pageIndex:int}/{pageSize:int}", GetPaged).WithTags(_featureGroupName);
        app.MapGet(Prefix + "/getById/{id:guid}", GetById).WithTags(_featureGroupName);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
    protected async Task<OperationResult<IEnumerable<TReadViewModel>>> GetAll(
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new GetAllRequest<TEntity, TReadViewModel>(
                context.User),
            context.RequestAborted);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
    protected async Task<OperationResult<IPagedList<TReadViewModel>>> GetPaged(
        [FromServices] IMediator mediator,
        HttpContext context, int pageIndex, int pageSize)
        => await mediator.Send(new GetPagedRequest<TEntity, TReadViewModel>(
                context.User, pageIndex, pageSize),
            context.RequestAborted);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
    protected async Task<OperationResult<TDetailsReadViewModel>> GetById(
        [FromServices] IMediator mediator,
        HttpContext context, Guid id)
        => await mediator.Send(new GetByIdRequest<TEntity, TDetailsReadViewModel>(
                context.User, id),
            context.RequestAborted);
}