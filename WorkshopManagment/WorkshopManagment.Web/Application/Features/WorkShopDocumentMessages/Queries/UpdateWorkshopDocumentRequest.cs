using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
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
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork = unitOfWork;


    public override async Task<OperationResult<UpdateWorkshopDocumentViewModel>> Handle(
        UpdateRequest<UpdateWorkshopDocumentViewModel, WorkshopDocument> request,
        CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<UpdateWorkshopDocumentViewModel>();
        var errorMessage = string.Format(_errorMessageFormat,
            nameof(WorkshopDocument),
            request.Model.Id);

        var document = await _unitOfWork.GetRepository<WorkshopDocument>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.Id,
                include: queryable => queryable
                    .Include(navigationPropertyPath: workshopDocument => workshopDocument.Tasks)
                    .ThenInclude(navigationPropertyPath: task => task.EmployeeTaskRepeats),
                disableTracking: false);

        if (document is null)
        {
            operation.AddError(new SewingFactoryNotFoundException(errorMessage + $"The entity {nameof(WorkshopDocument)} was not found"));

            return operation;
        }

        _mapper.Map(request.Model,
            document);

        document.ApplyUpdatedTasks(_mapper.Map<List<WorkshopTask>>(request.Model.WorkshopTasks),
            await _unitOfWork.DbContext.ProcessBasedEmployees.Where(predicate: x => x.Documents!.Contains(document))
                .Select(selector: x => x.Id)
                .ToListAsync(cancellationToken));

        _unitOfWork.GetRepository<WorkshopDocument>()
            .Update(document);

        await _unitOfWork.SaveChangesAsync();
        if (!_unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(_unitOfWork.LastSaveChangesResult.Exception);

            return operation;
        }

        operation.Result = request.Model;

        return operation;
    }
}