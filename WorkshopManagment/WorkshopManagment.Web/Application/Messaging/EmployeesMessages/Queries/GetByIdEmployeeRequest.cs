using Calabonga.OperationResults;
using Calabonga.PagedListCore;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries
{
    public record GetByIdEmployeeRequest(ClaimsPrincipal User, Guid Id) : IRequest<OperationResult<Employee>>;

    public class GetByIdEmployeeHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetByIdEmployeeRequest, OperationResult<Employee>>
    {
        public async Task<OperationResult<Employee>> Handle(
            GetByIdEmployeeRequest request,
            CancellationToken cancellationToken)
        {
            var result = OperationResult.CreateResult<Employee>();
            var employee = await unitOfWork.GetRepository<Employee>()
                .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id);

            if (employee == null)
            {
                result.AddError($"Employee with Id {request.Id} not found.");
                return result;
            }
            result.Result = employee;
            return result;
        }
    }
}