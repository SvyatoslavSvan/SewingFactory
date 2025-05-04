using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;

public record UpdateRequest<TViewModel, TEntity>(TViewModel Model, ClaimsPrincipal User) : IRequest<OperationResult<TViewModel>> where TViewModel : IIdentityViewModel where TEntity : Identity;

public abstract class UpdateHandler<TViewModel, TEntity>(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateRequest<TViewModel, TEntity>, OperationResult<TViewModel>>
    where TEntity : Identity
    where TViewModel : IIdentityViewModel
{
    private const string _errorMessageFormat = "An error occurred while updating {0} with Id {1}.";

    public async Task<OperationResult<TViewModel>> Handle(UpdateRequest<TViewModel, TEntity> request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<TViewModel>();
        var errorMessage = string.Format(_errorMessageFormat, nameof(TEntity), request.Model.Id);
        var repository = unitOfWork.GetRepository<TEntity>();
        var entity = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id, disableTracking: false);
        if (entity is null)
        {
            operation.AddError(new SewingFactoryNotFoundException(errorMessage + $"The entity {nameof(TEntity)} was not found"));

            return operation;
        }

        mapper.Map(request.Model, entity);
        repository.Update(entity);
        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(new SewingFactoryDatabaseSaveException(errorMessage));

            return operation;
        }

        operation.Result = request.Model;

        return operation;
    }
}