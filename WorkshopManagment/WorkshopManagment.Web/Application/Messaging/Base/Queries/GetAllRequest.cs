using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Common.Domain.Base;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;

public record GetAllRequest<T>(ClaimsPrincipal User) : IRequest<OperationResult<IEnumerable<T>>> where T : Identity;

public class GetAllHandler<T>(IUnitOfWork unitOfWork) : IRequestHandler<GetAllRequest<T>, OperationResult<IEnumerable<T>>> where T : Identity
{
    public async Task<OperationResult<IEnumerable<T>>> Handle(
        GetAllRequest<T> request,
        CancellationToken cancellationToken)
        => OperationResult.CreateResult<IEnumerable<T>>(await unitOfWork.GetRepository<T>()
            .GetAllAsync(true));
}