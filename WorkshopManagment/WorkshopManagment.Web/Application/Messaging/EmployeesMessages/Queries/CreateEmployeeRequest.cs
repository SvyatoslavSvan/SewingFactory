using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;

public record CreateEmployeeRequest(EmployeeCreateViewModel Model, ClaimsPrincipal User)
    : IRequest<OperationResult<EmployeeReadViewModel>>;

public class CreateEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateEmployeeRequest, OperationResult<EmployeeReadViewModel>>
{
    private static readonly Dictionary<EmployeeKind, Type> _typeMap = new()
    {
        [EmployeeKind.Process] = typeof(ProcessBasedEmployee), 
        [EmployeeKind.Rate] = typeof(RateBasedEmployee), 
        [EmployeeKind.Technologist] = typeof(Technologist)
    };

    public async Task<OperationResult<EmployeeReadViewModel>> Handle(CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var operationResult = OperationResult.CreateResult<EmployeeReadViewModel>();
        if (!_typeMap.TryGetValue(request.Model.GetEmployeeKind(), out var destinationType))
        {
            operationResult.AddError(new SewingFactoryArgumentNullException(nameof(request.Model.GetEmployeeKind)));

            return operationResult;
        }

        var employee = (Employee)mapper.Map(request.Model,
            typeof(EmployeeCreateViewModel),
            destinationType);

        await unitOfWork.GetRepository<Employee>().InsertAsync(employee, cancellationToken);
        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.LastSaveChangesResult.IsOk)
        {
            operationResult.AddError(unitOfWork.LastSaveChangesResult.Exception
                                     ?? new SewingFactoryDatabaseSaveException($"Error while saving entity{nameof(ProcessBasedEmployee)}"));

            return operationResult;
        }

        operationResult.Result = mapper.Map<EmployeeReadViewModel>(employee);

        return operationResult;
    }
}