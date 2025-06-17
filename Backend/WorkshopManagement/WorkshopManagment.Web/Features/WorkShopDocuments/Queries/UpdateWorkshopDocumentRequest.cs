using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.Mapping.Profiles;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

// ReSharper disable AsyncVoidLambda

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.Queries;

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

        var document = await LoadDocument(request);

        if (document is null)
        {
            operationResult.AddError(new SewingFactoryNotFoundException(
                $"WorkshopDocument with Id {request.Model.Id} not found"));
            return operationResult;
        }

        var employeeDictionary = await GetEmployeeDictionaryFromWorkshopTasks(request);

        await UpdateWorkshopDocument(request, document, employeeDictionary);
        if (!unitOfWork.Result.Ok)
        {
            operationResult.AddError(unitOfWork.Result.Exception);

            return operationResult;
        }

        operationResult.Result = request.Model;

        return operationResult;
    }

    private async Task UpdateWorkshopDocument(UpdateRequest<UpdateWorkshopDocumentViewModel, WorkshopDocument> request, WorkshopDocument document,
        Dictionary<Guid, Employee> employeeDictionary)
    {
        _mapper.Map(request.Model,
            document,
            opts: opts
                => opts.Items[WorkshopDocumentMappingProfile.EmployeeDictionaryKey] = employeeDictionary);
        await unitOfWork.SaveChangesAsync();
    }

    private async Task<WorkshopDocument?> LoadDocument(UpdateRequest<UpdateWorkshopDocumentViewModel, WorkshopDocument> request) => await unitOfWork.GetRepository<WorkshopDocument>()
        .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id,
            include: queryable => queryable
                .Include(navigationPropertyPath: workshopDocument => workshopDocument.Tasks)
                .ThenInclude(navigationPropertyPath: task => task.EmployeeTaskRepeats)
                .ThenInclude(navigationPropertyPath: x => x.WorkShopEmployee).Include(navigationPropertyPath: x => x.Employees),
            trackingType: TrackingType.Tracking);

    private async Task<Dictionary<Guid, Employee>> GetEmployeeDictionaryFromWorkshopTasks( // materialize IDs before an EF query—EF Core can’t translate complex nested LINQ to SQL
        UpdateRequest<UpdateWorkshopDocumentViewModel, WorkshopDocument> request)
    {
        var employeeIds = request.Model.WorkshopTasks
            .SelectMany(selector: vm => vm.EmployeeRepeats.Select(selector: r => r.EmployeeId))
            .Distinct()
            .ToList();

        var employees = await unitOfWork.GetRepository<Employee>()
            .GetAllAsync(
                predicate: e => employeeIds.Contains(e.Id),
                trackingType: TrackingType.Tracking
            );

        return employees.ToDictionary(keySelector: e => e.Id);
    }
}