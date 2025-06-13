using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Common.Domain.Base;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;

public record GetAllRequest<TEntity, TViewModel>(ClaimsPrincipal User) : IRequest<Operation<IEnumerable<TViewModel>, Exception>> where TEntity : Identity;

public abstract class GetAllRequestHandler<TEntity, TViewModel>(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllRequest<TEntity, TViewModel>, Operation<IEnumerable<TViewModel>, Exception>> where TEntity : Identity
{
    public virtual async Task<Operation<IEnumerable<TViewModel>, Exception>> Handle(
        GetAllRequest<TEntity, TViewModel> request,
        CancellationToken cancellationToken)
        => Operation.Result(mapper.Map<IEnumerable<TViewModel>>(await unitOfWork.GetRepository<TEntity>()
            .GetAllAsync(TrackingType.NoTracking)));
}
