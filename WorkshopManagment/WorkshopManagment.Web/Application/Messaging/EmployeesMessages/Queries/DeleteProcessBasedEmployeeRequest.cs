using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;

public record DeleteProcessBasedEmployeeRequest(DeleteProcessBasedEmployeeViewModel Model, ClaimsPrincipal User) : IRequest<OperationResult<DeleteProcessBasedEmployeeViewModel>>;

public class DeleteProcessBasedEmployeeHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProcessBasedEmployeeRequest, OperationResult<DeleteProcessBasedEmployeeViewModel>>
{
    private const string _errorMessageFormat = "An error occurred while deleting {0} with Id {1}. Please try again.";

    public async Task<OperationResult<DeleteProcessBasedEmployeeViewModel>> Handle(DeleteProcessBasedEmployeeRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<DeleteProcessBasedEmployeeViewModel>();
        var errorMessage = string.Format(_errorMessageFormat, nameof(ProcessBasedEmployee), request.Model.Id);
        var repository = unitOfWork.GetRepository<ProcessBasedEmployee>();
        repository.Delete(await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id) ??
                          throw new SewingFactoryNotFoundException(errorMessage));

        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(unitOfWork.LastSaveChangesResult.Exception
                               ?? new SewingFactoryDatabaseSaveException(errorMessage));

            return operation;
        }

        operation.Result = request.Model;

        return operation;
    }
}