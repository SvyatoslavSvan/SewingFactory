using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;

public record CreateRequest<TCreateViewModel, TEntity, TReadViewModel>(TCreateViewModel Model, ClaimsPrincipal User) : IRequest<OperationResult<TReadViewModel>>
{
};

public abstract class CreateRequestHandler<TCreateViewModel, TEntity, TReadViewModel>(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateRequest<TCreateViewModel, TEntity, TReadViewModel>, OperationResult<TReadViewModel>> where TEntity : Identity
{
    
    protected virtual Task AfterEntityCreatedAsync(TEntity entity) => Task.CompletedTask;
    
    public virtual async Task<OperationResult<TReadViewModel>> Handle(CreateRequest<TCreateViewModel, TEntity, TReadViewModel> request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<TReadViewModel>();
        var entity = await InsertEntity(request, cancellationToken);
        if (!unitOfWork.Result.Ok)
        {
            return AddDatabaseError(operation);
        }

        await AfterEntityCreatedAsync(entity);
        return MapEntityToReadViewModel(operation, entity);
    }

    private OperationResult<TReadViewModel> MapEntityToReadViewModel(OperationResult<TReadViewModel> operation, TEntity entity)
    {
        operation.Result = mapper.Map<TReadViewModel>(entity);

        return operation;
    }

    private OperationResult<TReadViewModel> AddDatabaseError(OperationResult<TReadViewModel> operation)
    {
        operation.AddError(unitOfWork.Result.Exception
                           ?? new SewingFactoryDatabaseSaveException($"Error while saving entity{nameof(TEntity)}"));

        return operation;
    }

    private async Task<TEntity> InsertEntity(CreateRequest<TCreateViewModel, TEntity, TReadViewModel> request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<TEntity>(request.Model);
        await unitOfWork.GetRepository<TEntity>().InsertAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync();
        return entity;
    }
}