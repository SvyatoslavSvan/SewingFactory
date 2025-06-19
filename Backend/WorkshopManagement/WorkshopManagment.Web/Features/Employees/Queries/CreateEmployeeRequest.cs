using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Queries;

public sealed record CreateEmployeeRequest(EmployeeCreateViewModel Model, ClaimsPrincipal User) : CreateRequest<EmployeeCreateViewModel, Employee, EmployeeReadViewModel>(Model, User);

public sealed class CreateEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper) : CreateRequestHandler<EmployeeCreateViewModel, Employee, EmployeeReadViewModel>(unitOfWork, mapper)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public override async Task<OperationResult<EmployeeReadViewModel>> Handle(CreateRequest<EmployeeCreateViewModel, Employee, EmployeeReadViewModel> request, CancellationToken cancellationToken)
        => await SaveEmployee(request, await LoadDepartment(request), cancellationToken);

    private async Task<OperationResult<EmployeeReadViewModel>> SaveEmployee(
        CreateRequest<EmployeeCreateViewModel, Employee, EmployeeReadViewModel> request,
        Department department,
        CancellationToken cancellationToken)
    {
        var employee = _mapper.Map<Employee>(request.Model,
            opts: opts => opts.Items[nameof(Department)] = department);

        await _unitOfWork.GetRepository<Employee>()
            .InsertAsync(employee,
                cancellationToken);

        await _unitOfWork.SaveChangesAsync();

        return EnsureEmployeeCreated(employee);
    }

    private OperationResult<EmployeeReadViewModel> EnsureEmployeeCreated(Employee employee)
    {
        var operationResult = new OperationResult<EmployeeReadViewModel>();
        if (!_unitOfWork.Result.Ok)
        {
            operationResult.AddError(_unitOfWork.Result.Exception!.Message, _unitOfWork.Result.Exception!);

            return operationResult;
        }

        operationResult.Result = _mapper.Map<EmployeeReadViewModel>(employee);

        return operationResult;
    }

    private async Task<Department> LoadDepartment(CreateRequest<EmployeeCreateViewModel, Employee, EmployeeReadViewModel> request)
    {
        var department = await _unitOfWork.GetRepository<Department>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.DepartmentId,
                trackingType: TrackingType.Tracking);

        if (department is null)
        {
            throw new SewingFactoryNotFoundException("Unable to create employee. Department not found");
        }

        return department;
    }
}