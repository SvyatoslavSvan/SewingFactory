using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.PagedListCore;
using Calabonga.UnitOfWork;
using MediatR;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;

public record GetPagedRequest<TEntity, TViewModel>(ClaimsPrincipal User, int PageIndex, int PageSize)
    : IRequest<OperationResult<IPagedList<TViewModel>>> where TEntity : class;

public abstract class GetPagedRequestHandler<TEntity, TViewModel>(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetPagedRequest<TEntity, TViewModel>, OperationResult<IPagedList<TViewModel>>> where TEntity : class
{
    public virtual async Task<OperationResult<IPagedList<TViewModel>>> Handle(GetPagedRequest<TEntity, TViewModel> request, CancellationToken cancellationToken)
        => OperationResult.CreateResult(mapper.Map<IPagedList<TViewModel>>(await unitOfWork.GetRepository<TEntity>()
            .GetPagedListAsync(pageIndex: request.PageIndex, pageSize: request.PageSize, cancellationToken: cancellationToken)));
}