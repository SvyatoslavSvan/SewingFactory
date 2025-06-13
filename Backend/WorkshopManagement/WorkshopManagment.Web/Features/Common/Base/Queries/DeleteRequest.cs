using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;

public record DeleteRequest<TViewModel, TEntity>(TViewModel Model, ClaimsPrincipal User) : IRequest<OperationResult<TViewModel>> where TEntity : Identity where TViewModel : IIdentityViewModel;

public abstract class DeleteRequestHandler<TViewModel, TEntity>(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteRequest<TViewModel, TEntity>, OperationResult<TViewModel>> where TEntity : Identity where TViewModel : IIdentityViewModel
{
    private const string _errorMessageFormat = "An error occurred while deleting {0} with Id {1}. Please try again.";

    public virtual async Task<OperationResult<TViewModel>> Handle(DeleteRequest<TViewModel, TEntity> request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<TViewModel>();
        var errorMessage = string.Format(_errorMessageFormat, typeof(TViewModel).Name, request.Model.Id);
        var repository = unitOfWork.GetRepository<TEntity>();
        repository.Delete(await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id) ??
                          throw new SewingFactoryNotFoundException(errorMessage));

        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.Result.Ok)
        {
            operation.AddError(unitOfWork.Result.Exception
                               ?? new SewingFactoryDatabaseSaveException(errorMessage));

            return operation;
        }

        operation.Result = request.Model;

        return operation;
    }
}