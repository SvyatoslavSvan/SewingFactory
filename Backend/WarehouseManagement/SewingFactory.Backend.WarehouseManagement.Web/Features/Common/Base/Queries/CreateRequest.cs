using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;

public record CreateRequest<TCreateViewModel, TEntity, TReadViewModel>(TCreateViewModel Model, ClaimsPrincipal User) : IRequest<Operation<TReadViewModel, Exception>>
{
};

public abstract class CreateRequestHandler<TCreateViewModel, TEntity, TReadViewModel>(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateRequest<TCreateViewModel, TEntity, TReadViewModel>, Operation<TReadViewModel, Exception>> where TEntity : Identity
{
    public virtual async Task<Operation<TReadViewModel, Exception>> Handle(CreateRequest<TCreateViewModel, TEntity, TReadViewModel> request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<TEntity>(request.Model);
        await unitOfWork.GetRepository<TEntity>().InsertAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.Result.Ok)
        {
            return Operation.Error(unitOfWork.Result.Exception
                                   ?? new SewingFactoryDatabaseSaveException($"Error while saving entity{nameof(TEntity)}"));
        }

        return Operation.Result(mapper.Map<TReadViewModel>(entity));
    }
}
