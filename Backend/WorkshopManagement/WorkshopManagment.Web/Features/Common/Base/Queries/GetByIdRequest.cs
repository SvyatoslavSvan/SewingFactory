using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Common.Domain.Base;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;

public record GetByIdRequest<TEntity, TViewModel>(ClaimsPrincipal User, Guid Id) : IRequest<OperationResult<TViewModel>> where TEntity : Identity;

public abstract class GetByIdRequestHandler<TEntity, TViewModel>(ApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetByIdRequest<TEntity, TViewModel>, OperationResult<TViewModel>> where TEntity : Identity
{
    public virtual async Task<OperationResult<TViewModel>> Handle(GetByIdRequest<TEntity, TViewModel> request, CancellationToken cancellationToken)
    {
        var result = OperationResult.CreateResult<TViewModel>();
        var entity = await LoadEntityWithNavigations(request, cancellationToken);

        if (entity == null)
        {
            return AddEntityNotFoundError(request, result);
        }

        result.Result = mapper.Map<TViewModel>(entity);

        return result;
    }

    private static OperationResult<TViewModel> AddEntityNotFoundError(GetByIdRequest<TEntity, TViewModel> request, OperationResult<TViewModel> result)
    {
        result.AddError($"{typeof(TEntity).Name} with Id {request.Id} not found.");

        return result;
    }

    private async Task<TEntity?> LoadEntityWithNavigations(GetByIdRequest<TEntity, TViewModel> request, CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = dbContext.Set<TEntity>();
        query = dbContext.Model.FindEntityType(typeof(TEntity))!.GetNavigations().Aggregate(query, func: (current, nav) => current.Include(nav.Name));
        var entity = await query.FirstOrDefaultAsync(predicate: x => x.Id == request.Id, cancellationToken);
        return entity;
    }
}