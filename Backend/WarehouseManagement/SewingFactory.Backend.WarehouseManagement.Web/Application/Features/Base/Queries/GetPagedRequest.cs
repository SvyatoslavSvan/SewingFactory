using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.PagedListCore;
using Calabonga.UnitOfWork;
using MediatR;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;

public record GetPagedRequest<TEntity, TViewModel>(ClaimsPrincipal User, int PageIndex, int PageSize)
    : IRequest<Operation<IPagedList<TViewModel>>> where TEntity : class;

public abstract class GetPagedRequestHandler<TEntity, TViewModel>(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetPagedRequest<TEntity, TViewModel>, Operation<IPagedList<TViewModel>>> where TEntity : class
{
    public virtual async Task<Operation<IPagedList<TViewModel>>> Handle(GetPagedRequest<TEntity, TViewModel> request, CancellationToken cancellationToken)
        => Operation.Result(mapper.Map<IPagedList<TViewModel>>(await unitOfWork.GetRepository<TEntity>()
            .GetPagedListAsync(pageIndex: request.PageIndex, pageSize: request.PageSize, cancellationToken: cancellationToken)));
}
