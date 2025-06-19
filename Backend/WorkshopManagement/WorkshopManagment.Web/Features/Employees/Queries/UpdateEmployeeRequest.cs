using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Queries;

public sealed record UpdateEmployeeRequest(EmployeeUpdateViewModel Model, ClaimsPrincipal User)
    : UpdateRequest<EmployeeUpdateViewModel, Employee>(Model, User);

public sealed class UpdateEmployeeRequestHandler(IUnitOfWork unitOfWork, IMapper mapper) : UpdateRequestHandler<EmployeeUpdateViewModel, Employee>(unitOfWork, mapper)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public override async Task<OperationResult<EmployeeUpdateViewModel>> Handle(UpdateRequest<EmployeeUpdateViewModel, Employee> request, CancellationToken cancellationToken)
    {
        var employee = await LoadEmployee(request);
        await UpdateEmployee(request, employee);
        return EnsureEmployeeUpdated(request);
    }

    private OperationResult<EmployeeUpdateViewModel> EnsureEmployeeUpdated(UpdateRequest<EmployeeUpdateViewModel, Employee> request)
    {
        var operationResult = new OperationResult<EmployeeUpdateViewModel>();
        if (!_unitOfWork.Result.Ok)
        {
            operationResult.AddError("Unable to update employee", _unitOfWork.Result.Exception ?? new Exception("Unknown error"));;    
        }
        operationResult.Result = request.Model;
        return operationResult;
    }

    private async Task<Employee> LoadEmployee(UpdateRequest<EmployeeUpdateViewModel, Employee> request)
    {
        var employee = await _unitOfWork.GetRepository<Employee>()
            .GetFirstOrDefaultAsync(predicate: e => e.Id == request.Model.Id,
                include: queryable => queryable.Include(e => e.Department), 
                trackingType: TrackingType.Tracking);

        if (employee is null)
        {
            throw new SewingFactoryNotFoundException("Unable to update employee. Employee not found");
        }

        return employee;
    }

    private async Task UpdateEmployee(UpdateRequest<EmployeeUpdateViewModel, Employee> request, Employee employee)
    {
        if (request.Model.DepartmentId  != employee.Department.Id)
        {
            employee.Department = await _unitOfWork.GetRepository<Department>()
                                      .GetFirstOrDefaultAsync(predicate: d => d.Id == request.Model.DepartmentId,
                                          trackingType: TrackingType.Tracking) ??
                                  throw new SewingFactoryNotFoundException("Unable to update employee with department which not exist");
        }
        _mapper.Map(request.Model, employee);
        await _unitOfWork.SaveChangesAsync();
    }
}