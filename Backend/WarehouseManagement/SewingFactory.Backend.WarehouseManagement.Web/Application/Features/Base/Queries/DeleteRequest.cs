using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;

public record DeleteRequest<TViewModel, TEntity>(TViewModel Model, ClaimsPrincipal User) : IRequest<Operation<TViewModel, Exception>> where TEntity : Identity where TViewModel : IIdentityViewModel;

public abstract class DeleteRequestHandler<TViewModel, TEntity>(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteRequest<TViewModel, TEntity>, Operation<TViewModel, Exception>> where TEntity : Identity where TViewModel : IIdentityViewModel
{
    private const string _errorMessageFormat = "An error occurred while deleting {0} with Id {1}. Please try again.";

    public virtual async Task<Operation<TViewModel, Exception>> Handle(DeleteRequest<TViewModel, TEntity> request, CancellationToken cancellationToken)
    {
        var errorMessage = string.Format(_errorMessageFormat, typeof(TViewModel).Name, request.Model.Id);
        var repository = unitOfWork.GetRepository<TEntity>();
        repository.Delete(await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id) ??
                          throw new SewingFactoryNotFoundException(errorMessage));

        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.Result.Ok)
        {
            Operation.Error(unitOfWork.Result.Exception
                            ?? new SewingFactoryDatabaseSaveException(errorMessage));
        }

        return Operation.Result(request.Model);
    }
}
