using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Common.Domain.Base;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;

public record GetAllRequest<TEntity, TViewModel>(ClaimsPrincipal User) : IRequest<OperationResult<IEnumerable<TViewModel>>> where TEntity : Identity;

public abstract class GetAllRequestHandler<TEntity, TViewModel>(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllRequest<TEntity, TViewModel>, OperationResult<IEnumerable<TViewModel>>> where TEntity : Identity
{
    public virtual async Task<OperationResult<IEnumerable<TViewModel>>> Handle(
        GetAllRequest<TEntity, TViewModel> request,
        CancellationToken cancellationToken)
        => OperationResult.CreateResult(mapper.Map<IEnumerable<TViewModel>>(await unitOfWork.GetRepository<TEntity>()
            .GetAllAsync(TrackingType.NoTracking)));
}