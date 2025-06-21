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
    public virtual async Task<OperationResult<TViewModel>> Handle(UpdateRequest<TViewModel, TEntity> request, CancellationToken cancellationToken)
    {
        if (await LoadEntity(request) is not { } entity)
        {
            return AddEntityNotFoundError(request.Model.Id);
        }

        await UpdateEntity(request, entity);

        return EnsureUpdated(request);
    }

    protected OperationResult<TViewModel> EnsureUpdated(UpdateRequest<TViewModel, TEntity> request)
    {
        var operation = OperationResult.CreateResult<TViewModel>();
        if (!unitOfWork.Result.Ok)
        {
            return AddDatabaseSaveError(operation, operation.Exception ?? new Exception("Unknown exception"));
        }

        operation.Result = request.Model;

        return operation;
    }

    protected static OperationResult<TViewModel> AddEntityNotFoundError(Guid entityId)
    {
        var operation = OperationResult.CreateResult<TViewModel>();
        operation.AddError(new SewingFactoryNotFoundException($"The entity {nameof(TEntity)} with {entityId} was not found"));

        return operation;
    }

    private static OperationResult<TViewModel> AddDatabaseSaveError(OperationResult<TViewModel> operation, Exception exception)
    {
        operation.AddError(exception.Message, exception);
        ;

        return operation;
    }

    protected async Task UpdateEntity(UpdateRequest<TViewModel, TEntity> request, TEntity entity)
    {
        mapper.Map(request.Model, entity);
        await unitOfWork.SaveChangesAsync();
    }

    protected async Task<TEntity?> LoadEntity(UpdateRequest<TViewModel, TEntity> request) => await unitOfWork.GetRepository<TEntity>()
        .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id,
            trackingType: TrackingType.Tracking);
}