using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels.Base;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;

public record CreateProcessBasedEmployeeRequest(ProcessBasedEmployeeViewModel Model, ClaimsPrincipal User) : IRequest<OperationResult<IdentityProcessBasedEmployeeViewModel>>;

public class CreateProcessBasedEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateProcessBasedEmployeeRequest, OperationResult<IdentityProcessBasedEmployeeViewModel>>
{
    public async Task<OperationResult<IdentityProcessBasedEmployeeViewModel>> Handle(CreateProcessBasedEmployeeRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<IdentityProcessBasedEmployeeViewModel>();
        var employee = mapper.Map<ProcessBasedEmployee>(request.Model);
        await unitOfWork.GetRepository<ProcessBasedEmployee>().InsertAsync(employee, cancellationToken);
        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(unitOfWork.LastSaveChangesResult.Exception
                               ?? new SewingFactoryDatabaseSaveException($"Error while saving entity{nameof(ProcessBasedEmployee)}"));

            return operation;
        }

        operation.Result = mapper.Map<IdentityProcessBasedEmployeeViewModel>(employee);

        return operation;
    }
}