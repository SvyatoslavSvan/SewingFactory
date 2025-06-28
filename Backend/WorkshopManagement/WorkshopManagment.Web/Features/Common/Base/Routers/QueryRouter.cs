using Calabonga.AspNetCore.AppDefinitions;
using Calabonga.OperationResults;
using Calabonga.PagedListCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Routers;

public abstract class QueryRouter<TEntity, TReadViewModel, TDetailsReadViewModel> : AppDefinition where TEntity : Identity
{
    private static readonly string _featureGroupName = typeof(TEntity).Name;
    private string Prefix => "/api/" + _featureGroupName;
    protected virtual string? PolicyName => null;

    protected static RouteGroupBuilder? _group;

    public override void ConfigureApplication(WebApplication app)
    {
        ConfigureGroup(app);
        MapQueryRoutes();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    protected async Task<OperationResult<IEnumerable<TReadViewModel>>> GetAll(
        [FromServices] IMediator mediator,
        HttpContext context)
        => await mediator.Send(new GetAllRequest<TEntity, TReadViewModel>(
                context.User),
            context.RequestAborted);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    protected async Task<OperationResult<IPagedList<TReadViewModel>>> GetPaged(
        [FromServices] IMediator mediator,
        HttpContext context, int pageIndex, int pageSize)
        => await mediator.Send(new GetPagedRequest<TEntity, TReadViewModel>(
                context.User, pageIndex, pageSize),
            context.RequestAborted);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    protected async Task<OperationResult<TDetailsReadViewModel>> GetById(
        [FromServices] IMediator mediator,
        HttpContext context, Guid id)
        => await mediator.Send(new GetByIdRequest<TEntity, TDetailsReadViewModel>(
                context.User, id),
            context.RequestAborted);

    private void MapQueryRoutes()
    {
        if (_group == null)
        {
            throw new SewingFactoryInvalidOperationException("Cannot configure routes. Group is null.");
        }

        _group.MapGet("/getAll", GetAll);
        _group.MapGet("/getPaged/{pageIndex:int}/{pageSize:int}", GetPaged);
        _group.MapGet("/getById/{id:guid}", GetById);
    }

    private void ConfigureGroup(WebApplication app)
    {
        _group = app.MapGroup(Prefix).WithTags(_featureGroupName);
        if (!string.IsNullOrWhiteSpace(PolicyName))
        {
            _group.RequireAuthorization(PolicyName);
        }
    }
}