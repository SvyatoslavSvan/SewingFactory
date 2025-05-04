using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Common.Domain.Base;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;

public record GetByIdRequest<TEntity, TViewModel>(ClaimsPrincipal User, Guid Id) : IRequest<OperationResult<TViewModel>> where TEntity : Identity;

public abstract class GetByIdHandler<TEntity, TViewModel>(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetByIdRequest<TEntity, TViewModel>, OperationResult<TViewModel>> where TEntity : Identity
{
    public virtual async Task<OperationResult<TViewModel>> Handle(GetByIdRequest<TEntity, TViewModel> request, CancellationToken cancellationToken)
    {
        var result = OperationResult.CreateResult<TViewModel>();
        var employee = await unitOfWork.GetRepository<TEntity>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id);

        if (employee == null)
        {
            result.AddError($"{typeof(TEntity).Name} with Id {request.Id} not found.");

            return result;
        }

        result.Result = mapper.Map<TViewModel>(employee);

        return result;
    }
}