using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;

public record UpdateRequest<TViewModel, TEntity>(TViewModel Model, ClaimsPrincipal User) : IRequest<OperationResult<TViewModel>> where TViewModel : IIdentityViewModel where TEntity : Identity;

public abstract class UpdateRequestHandler<TViewModel, TEntity>(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateRequest<TViewModel, TEntity>, OperationResult<TViewModel>>
    where TEntity : Identity
    where TViewModel : IIdentityViewModel
{
    private const string _errorMessageFormat = "An error occurred while updating {0} with Id {1}.";

    protected virtual Task AfterEntityUpdatedAsync(TEntity entity) => Task.CompletedTask;
    
    public virtual async Task<OperationResult<TViewModel>> Handle(UpdateRequest<TViewModel, TEntity> request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<TViewModel>();
        var errorMessage = string.Format(_errorMessageFormat, nameof(TEntity), request.Model.Id);
        var entity = await LoadEntity(request);
        if (entity is null)
        {
            return AddEntityNotFoundError(operation, errorMessage);
        }

        await UpdateEntity(request, entity);
        if (!unitOfWork.Result.Ok)
        {
            return AddDatabaseSaveError(operation, errorMessage);
        }
        await AfterEntityUpdatedAsync(entity);
        operation.Result = request.Model;

        return operation;
    }

    private static OperationResult<TViewModel> AddEntityNotFoundError(OperationResult<TViewModel> operation, string errorMessage)
    {
        operation.AddError(new SewingFactoryNotFoundException(errorMessage + $"The entity {nameof(TEntity)} was not found"));

        return operation;
    }

    private static OperationResult<TViewModel> AddDatabaseSaveError(OperationResult<TViewModel> operation, string errorMessage)
    {
        operation.AddError(new SewingFactoryDatabaseSaveException(errorMessage));

        return operation;
    }

    private async Task UpdateEntity(UpdateRequest<TViewModel, TEntity> request, TEntity entity)
    {
        mapper.Map(request.Model, entity);
        await unitOfWork.SaveChangesAsync();
    }

    private async Task<TEntity?> LoadEntity(UpdateRequest<TViewModel, TEntity> request) => await unitOfWork.GetRepository<TEntity>()
        .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id,
            trackingType: TrackingType.Tracking);
}