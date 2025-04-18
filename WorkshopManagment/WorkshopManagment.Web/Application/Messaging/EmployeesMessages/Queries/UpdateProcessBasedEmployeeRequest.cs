using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels.Base;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;

public record UpdateProcessBasedEmployeeRequest(IdentityProcessBasedEmployeeViewModel Model, ClaimsPrincipal User) : IRequest<OperationResult<IdentityProcessBasedEmployeeViewModel>>;

public class UpdateProcessBasedEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateProcessBasedEmployeeRequest, OperationResult<IdentityProcessBasedEmployeeViewModel>>
{
    private const string _errorMessageFormat = "An error occurred while updating {0} with Id {1}.";

    public async Task<OperationResult<IdentityProcessBasedEmployeeViewModel>> Handle(UpdateProcessBasedEmployeeRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<IdentityProcessBasedEmployeeViewModel>();
        var errorMessage = string.Format(_errorMessageFormat, nameof(ProcessBasedEmployee), request.Model.Id);
        var repository = unitOfWork.GetRepository<ProcessBasedEmployee>();
        var entity = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id, disableTracking: false);
        if (entity is null)
        {
            operation.AddError(new SewingFactoryNotFoundException(errorMessage + "The entity was not found"));

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