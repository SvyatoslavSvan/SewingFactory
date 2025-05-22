using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.Queries;

public sealed record GetPagedGarmentCategoryRequest(ClaimsPrincipal User, int PageIndex, int PageSize) : GetPagedRequest<GarmentCategory, ReadGarmentCategoryViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedGarmentCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper) : GetPagedRequestHandler<GarmentCategory, ReadGarmentCategoryViewModel>(unitOfWork, mapper)
{
}