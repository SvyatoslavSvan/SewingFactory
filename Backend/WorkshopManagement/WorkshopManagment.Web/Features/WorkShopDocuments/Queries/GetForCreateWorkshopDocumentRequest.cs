﻿using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.Queries;

public record GetForCreateWorkshopDocumentRequest(ClaimsPrincipal User) : IRequest<OperationResult<GetForCreateWorkshopDocumentViewModel>>;

public class GetForCreateWorkshopDocumentRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetForCreateWorkshopDocumentRequest, OperationResult<GetForCreateWorkshopDocumentViewModel>>
{
    public async Task<OperationResult<GetForCreateWorkshopDocumentViewModel>> Handle(GetForCreateWorkshopDocumentRequest request, CancellationToken cancellationToken) => OperationResult.CreateResult(
        new GetForCreateWorkshopDocumentViewModel
        {
            GarmentModel = mapper.Map<List<ReadGarmentModelViewModel>>(await unitOfWork.GetRepository<GarmentModel>().GetAllAsync(TrackingType.NoTracking)),
            Departments = mapper.Map<List<ReadDepartmentViewModel>>(await unitOfWork.GetRepository<Department>().GetAllAsync(TrackingType.NoTracking))
        });
}