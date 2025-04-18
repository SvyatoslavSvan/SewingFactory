using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries
{
    public record GetAllEmployeesRequest(ClaimsPrincipal User) : IRequest<OperationResult<IEnumerable<Employee>>>;

    public class GetAllEmployeesHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllEmployeesRequest, OperationResult<IEnumerable<Employee>>>
    {
        public async Task<OperationResult<IEnumerable<Employee>>> Handle(
            GetAllEmployeesRequest request,
            CancellationToken cancellationToken)
            => OperationResult.CreateResult<IEnumerable<Employee>>(await unitOfWork.GetRepository<ProcessBasedEmployee>()
                .GetAllAsync(disableTracking: true));
    }
}
