using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.Queries;

public sealed record CreateWorkshopDocumentRequest(
    CreateWorkshopDocumentViewModel Model,
    ClaimsPrincipal User)
    : CreateRequest<CreateWorkshopDocumentViewModel, WorkshopDocument, DetailsReadWorkshopDocumentViewModel>(Model,
        User);

public sealed class CreateWorkshopDocumentHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : CreateRequestHandler<CreateWorkshopDocumentViewModel, WorkshopDocument, DetailsReadWorkshopDocumentViewModel>(unitOfWork,
        mapper)
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public override async Task<OperationResult<DetailsReadWorkshopDocumentViewModel>> Handle(
        CreateRequest<CreateWorkshopDocumentViewModel, WorkshopDocument, DetailsReadWorkshopDocumentViewModel> request,
        CancellationToken cancellationToken)
    {
        var result = OperationResult.CreateResult<DetailsReadWorkshopDocumentViewModel>();
        var garmentModel = await _unitOfWork.GetRepository<GarmentModel>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.GarmentModelId,
                include: x => x.Include(navigationPropertyPath: model => model.Processes).ThenInclude(navigationPropertyPath: process => process.Department),
                trackingType: TrackingType.Tracking);

        var department = await _unitOfWork.GetRepository<Department>().GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.DepartmentId, trackingType: TrackingType.Tracking);
        if (garmentModel == null || department == null)
        {
            result.AddError(
                $"The {nameof(GarmentModel)} or {nameof(Department)} was not found with keys {request.Model.GarmentModelId} or {request.Model.DepartmentId}, unable to create {nameof(WorkshopDocument)}");

            return result;
        }

        var document = WorkshopDocument.CreateInstance(request.Model.Name,
            request.Model.CountOfModelsInvolved,
            request.Model.Date,
            garmentModel,
            department
        );

        await _unitOfWork.GetRepository<WorkshopDocument>()
            .InsertAsync(document,
                cancellationToken);

        await _unitOfWork.SaveChangesAsync();
        if (_unitOfWork.Result.Ok)
        {
            result.Result = _mapper.Map<DetailsReadWorkshopDocumentViewModel>(document);

            return result;
        }

        result.AddError(_unitOfWork.Result.Exception ?? new SewingFactoryDatabaseSaveException($"Error while saving entity{nameof(GarmentModel)}"));

        return result;
    }
}