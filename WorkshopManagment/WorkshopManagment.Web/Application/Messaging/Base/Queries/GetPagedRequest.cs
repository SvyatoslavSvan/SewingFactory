using Calabonga.OperationResults;
using Calabonga.PagedListCore;
using Calabonga.UnitOfWork;
using MediatR;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries
{
    public record GetPagedRequest<T>(ClaimsPrincipal User, int PageIndex, int PageSize) : IRequest<OperationResult<IPagedList<T>>> where T : class;

    public class GetPagedHandler<T>(IUnitOfWork unitOfWork) : IRequestHandler<GetPagedRequest<T>, OperationResult<IPagedList<T>>> where T : class
    {
        public async Task<OperationResult<IPagedList<T>>> Handle(
            GetPagedRequest<T> request,
            CancellationToken cancellationToken)
            => OperationResult.CreateResult(await unitOfWork.GetRepository<T>()
                .GetPagedListAsync(pageIndex: request.PageIndex, pageSize: request.PageSize, cancellationToken: cancellationToken));

    }
}