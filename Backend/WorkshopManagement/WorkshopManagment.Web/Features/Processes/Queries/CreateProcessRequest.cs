using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.Queries;

public sealed record CreateProcessRequest(CreateProcessViewModel Model, ClaimsPrincipal User)
    : CreateRequest<CreateProcessViewModel, Process, ReadProcessViewModel>(Model, User);

public sealed class CreateProcessHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : CreateRequestHandler<CreateProcessViewModel, Process, ReadProcessViewModel>(unitOfWork, mapper)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public override async Task<OperationResult<ReadProcessViewModel>> Handle(CreateRequest<CreateProcessViewModel, Process, ReadProcessViewModel> request, CancellationToken cancellationToken)
    {
        var department = await LoadDepartment(request);
        var process = await SaveDepartment(request, cancellationToken, department);
        return EnsureProcessCreated(process);
    }

    private async Task<Process> SaveDepartment(CreateRequest<CreateProcessViewModel, Process, ReadProcessViewModel> request, CancellationToken cancellationToken, Department department)
    {
        var process = _mapper.Map<Process>(source: request.Model,
            opts: opts => opts.Items[nameof(Department)] = department);
        await _unitOfWork.GetRepository<Process>().InsertAsync(process, cancellationToken);
        await _unitOfWork.SaveChangesAsync();
        return process;
    }

    private OperationResult<ReadProcessViewModel> EnsureProcessCreated(Process process)
    {
        var operationResult = new OperationResult<ReadProcessViewModel>();
        if (!_unitOfWork.Result.Ok)
        {
            operationResult.AddError(_unitOfWork.Result.Exception!.Message, _unitOfWork.Result.Exception!);

            return operationResult;
        }
        operationResult.Result = _mapper.Map<ReadProcessViewModel>(process);
        return operationResult;
    }

    private async Task<Department> LoadDepartment(CreateRequest<CreateProcessViewModel, Process, ReadProcessViewModel> request)
    {
        var department = await _unitOfWork.GetRepository<Department>()
            .GetFirstOrDefaultAsync(
                predicate: d => d.Id == request.Model.DepartmentId,
                trackingType: TrackingType.Tracking);
        if (department is null)
        {
            throw new SewingFactoryNotFoundException($"Unable to create process. Department not found {request.Model.DepartmentId}");
        }
        return department;
    }
}