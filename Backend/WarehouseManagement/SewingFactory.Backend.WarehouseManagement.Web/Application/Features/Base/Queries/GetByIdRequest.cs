using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;

public record GetByIdRequest<TEntity, TViewModel>(ClaimsPrincipal User, Guid Id) : IRequest<Operation<TViewModel, SewingFactoryNotFoundException>> where TEntity : Identity;

public abstract class GetByIdRequestHandler<TEntity, TViewModel>(IUnitOfWork<ApplicationDbContext> unitOfWork, IMapper mapper)
    : IRequestHandler<GetByIdRequest<TEntity, TViewModel>, Operation<TViewModel, SewingFactoryNotFoundException>> where TEntity : Identity
{
    public virtual async Task<Operation<TViewModel, SewingFactoryNotFoundException>> Handle(GetByIdRequest<TEntity, TViewModel> request, CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = unitOfWork.DbContext.Set<TEntity>();
        query = unitOfWork.DbContext.Model.FindEntityType(typeof(TEntity))!.GetNavigations().Aggregate(query, func: (current, nav) => current.Include(nav.Name));
        var entity = await query.FirstOrDefaultAsync(predicate: x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            return Operation.Error(new SewingFactoryNotFoundException($"{typeof(TEntity).Name} with Id {request.Id} not found."));
        }

        return Operation.Result(mapper.Map<TViewModel>(entity));
    }
}
