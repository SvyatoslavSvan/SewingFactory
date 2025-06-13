using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.Queries;

public sealed record GetPagedWorkshopDocumentRequest(
    ClaimsPrincipal User,
    int PageIndex,
    int PageSize)
    : GetPagedRequest<WorkshopDocument, ReadWorkshopDocumentViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedWorkshopDocumentHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : GetPagedRequestHandler<WorkshopDocument, ReadWorkshopDocumentViewModel>(unitOfWork, mapper);