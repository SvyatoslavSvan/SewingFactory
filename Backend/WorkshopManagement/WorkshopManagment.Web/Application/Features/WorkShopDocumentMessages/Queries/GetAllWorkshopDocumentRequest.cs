using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Document;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.Queries;

public sealed record GetAllWorkshopDocumentRequest(ClaimsPrincipal User)
    : GetAllRequest<WorkshopDocument, ReadWorkshopDocumentViewModel>(User);

public sealed class GetAllWorkshopDocumentHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : GetAllRequestHandler<WorkshopDocument, ReadWorkshopDocumentViewModel>(unitOfWork, mapper);