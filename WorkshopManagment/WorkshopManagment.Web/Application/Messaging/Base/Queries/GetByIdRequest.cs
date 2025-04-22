using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Common.Domain.Base;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries
{
    public record GetByIdRequest<T>(ClaimsPrincipal User, Guid Id) : IRequest<OperationResult<T>> where T : Identity;

    public class GetByIdHandler<T>(IUnitOfWork unitOfWork) : IRequestHandler<GetByIdRequest<T>, OperationResult<T>> where T : Identity
    {
        public virtual async Task<OperationResult<T>> Handle(GetByIdRequest<T> request, CancellationToken cancellationToken)
        {
            var result = OperationResult.CreateResult<T>();
            var employee = await unitOfWork.GetRepository<T>()
                .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id);

            if (employee == null)
            {
                result.AddError($"{typeof(T).Name} with Id {request.Id} not found.");
                return result;
            }
            result.Result = employee;
            return result;
        }
    }
}