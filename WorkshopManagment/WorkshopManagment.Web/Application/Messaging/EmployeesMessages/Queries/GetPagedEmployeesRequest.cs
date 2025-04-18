using Calabonga.OperationResults;
using Calabonga.PagedListCore;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries
{
    public record GetPagedEmployeesRequest(ClaimsPrincipal User, int PageIndex, int PageSize) : IRequest<OperationResult<IPagedList<Employee>>>;

    public class GetPagedEmployeesHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPagedEmployeesRequest, OperationResult<IPagedList<Employee>>>
    {
        public async Task<OperationResult<IPagedList<Employee>>> Handle(
            GetPagedEmployeesRequest request,
            CancellationToken cancellationToken)
            => OperationResult.CreateResult<IPagedList<Employee>>(await unitOfWork.GetRepository<Employee>()
                .GetPagedListAsync(pageIndex: request.PageIndex, pageSize: request.PageSize, cancellationToken: cancellationToken ));
    }
}