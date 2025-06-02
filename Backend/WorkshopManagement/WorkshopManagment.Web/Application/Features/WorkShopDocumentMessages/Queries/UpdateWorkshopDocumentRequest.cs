using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Document;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.Queries;

public sealed record UpdateWorkshopDocumentRequest(
    UpdateWorkshopDocumentViewModel Model,
    ClaimsPrincipal User)
    : UpdateRequest<UpdateWorkshopDocumentViewModel, WorkshopDocument>(Model,
        User);

public sealed class UpdateWorkshopDocumentHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper) : UpdateRequestHandler<UpdateWorkshopDocumentViewModel, WorkshopDocument>(unitOfWork,
    mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task<OperationResult<UpdateWorkshopDocumentViewModel>> Handle(
        UpdateRequest<UpdateWorkshopDocumentViewModel, WorkshopDocument> request,
        CancellationToken cancellationToken)
    {
        var operationResult = OperationResult.CreateResult<UpdateWorkshopDocumentViewModel>();
        var errorMessage = string.Format(_errorMessageFormat,
            nameof(WorkshopDocument),
            request.Model.Id);

        var document = await unitOfWork.GetRepository<WorkshopDocument>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id,
                include: queryable => queryable
                    .Include(navigationPropertyPath: workshopDocument => workshopDocument.Tasks)
                    .ThenInclude(navigationPropertyPath: task => task.EmployeeTaskRepeats)
                    .ThenInclude(navigationPropertyPath: x => x.WorkShopEmployee).Include(navigationPropertyPath: x => x.Employees),
                trackingType: TrackingType.Tracking);

        if (document is null)
        {
            operationResult.AddError(new SewingFactoryNotFoundException(errorMessage + $"The entity {nameof(WorkshopDocument)} was not found"));

            return operationResult;
        }

        _mapper.Map(request.Model,
            document);

        var employeesInvolvedIds = request.Model.WorkshopTasks.SelectMany(selector: x => x.EmployeeRepeats)
            .Select(selector: x => x.EmployeeId)
            .Distinct().ToList();

        var employeesInvolved = await unitOfWork.GetRepository<Employee>()
            .GetAllAsync(predicate: x => employeesInvolvedIds.Contains(x.Id),
                trackingType: TrackingType.Tracking);

        var employeesDictionary = employeesInvolved.ToDictionary(keySelector: x => x.Id);

        foreach (var taskVm in request.Model.WorkshopTasks)
        {
            var task = document.Tasks.FirstOrDefault(predicate: x => x.Id == taskVm.Id);
            if (task == null)
            {
                operationResult.AddError(new SewingFactoryNotFoundException($"Task {taskVm.Id} not found in document {document.Id}"));

                return operationResult;
            }

            var employeeTaskRepeats = _mapper.Map<List<EmployeeTaskRepeat>>(taskVm.EmployeeRepeats,
                opts: opt => opt.Items["EmployeesById"] = employeesDictionary);

            task.ReplaceRepeats(employeeTaskRepeats);
        }

        document.RecalculateEmployees();

        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.Result.Ok)
        {
            operationResult.AddError(unitOfWork.Result.Exception);

            return operationResult;
        }

        operationResult.Result = request.Model;

        return operationResult;
    }
}