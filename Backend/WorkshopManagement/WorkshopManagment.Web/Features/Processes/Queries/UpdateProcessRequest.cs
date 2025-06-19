using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.Queries;

public sealed record UpdateProcessRequest(
    UpdateProcessViewModel Model,
    ClaimsPrincipal User)
    : UpdateRequest<UpdateProcessViewModel, Process>(Model,
        User);

public sealed class UpdateProcessHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : UpdateRequestHandler<UpdateProcessViewModel, Process>(unitOfWork,
        mapper)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public override async Task<OperationResult<UpdateProcessViewModel>> Handle(
        UpdateRequest<UpdateProcessViewModel, Process> request,
        CancellationToken cancellationToken)
    {
        var process = await LoadProcess(request);
        await UpdateProcess(request,
            process);
        return EnsureProcessUpdated(request);
    }

    private OperationResult<UpdateProcessViewModel> EnsureProcessUpdated(
        UpdateRequest<UpdateProcessViewModel, Process> request)
    {
        var operationResult = new OperationResult<UpdateProcessViewModel>();
        if (!_unitOfWork.Result.Ok)
        {
            operationResult.AddError(operationResult.Exception == null
                    ? "Unknown Error"
                    : operationResult.Exception.Message,
                operationResult.Exception ?? new Exception("Unknown error"));

            return operationResult;
        }

        operationResult.Result = request.Model;

        return operationResult;
    }

    private async Task UpdateProcess(
        UpdateRequest<UpdateProcessViewModel, Process> request,
        Process process)
    {
        if (process.Department.Id != request.Model.DepartmentId)
        {
            process.Department = await _unitOfWork.GetRepository<Department>()
                                     .GetFirstOrDefaultAsync(predicate: d => d.Id == request.Model.DepartmentId,
                                         trackingType: TrackingType.Tracking) ??
                                 throw new SewingFactoryNotFoundException($"Unable to update process with department {request.Model.DepartmentId} which not exist");
        }

        _mapper.Map(request.Model,
            process);

        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<Process> LoadProcess(
        UpdateRequest<UpdateProcessViewModel, Process> request)
    {
        var process = await _unitOfWork.GetRepository<Process>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id,
                include: q => q.Include(navigationPropertyPath: p => p.Department),
                trackingType: TrackingType.Tracking);

        if (process is null)
        {
            throw new SewingFactoryNotFoundException($"Entity Process {request.Model.Id} not found. Unable to update.");
        }
        return process;
    }
}