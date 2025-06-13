using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;

public record UpdateRequest<TViewModel, TEntity>(TViewModel Model, ClaimsPrincipal User)
    : IRequest<Operation<TViewModel, SewingFactoryNotFoundException, SewingFactoryDatabaseSaveException>> where TViewModel : IIdentityViewModel where TEntity : Identity;

public abstract class UpdateRequestHandler<TViewModel, TEntity>(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateRequest<TViewModel, TEntity>, Operation<TViewModel, SewingFactoryNotFoundException, SewingFactoryDatabaseSaveException>>
    where TEntity : Identity
    where TViewModel : IIdentityViewModel
{
    protected const string _errorMessageFormat = "An error occurred while updating {0} with Id {1}.";

    public virtual async Task<Operation<TViewModel, SewingFactoryNotFoundException, SewingFactoryDatabaseSaveException>> Handle(
        UpdateRequest<TViewModel, TEntity> request, CancellationToken cancellationToken)
    {
        var errorMessage = string.Format(_errorMessageFormat, nameof(TEntity), request.Model.Id);
        var repository = unitOfWork.GetRepository<TEntity>();
        var entity = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id, trackingType: TrackingType.Tracking);
        if (entity is null)
        {
            return Operation.Error(new SewingFactoryNotFoundException(errorMessage + $"The entity {nameof(TEntity)} was not found"));
        }

        mapper.Map(request.Model, entity);
        repository.Update(entity);
        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.Result.Ok)
        {
            return Operation.Error(new SewingFactoryDatabaseSaveException(errorMessage));
        }

        return Operation.Result(request.Model);
    }
}
