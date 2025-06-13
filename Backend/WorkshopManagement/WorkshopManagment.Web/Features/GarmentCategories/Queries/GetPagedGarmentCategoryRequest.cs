using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Queries;

public sealed record GetPagedGarmentCategoryRequest(ClaimsPrincipal User, int PageIndex, int PageSize) : GetPagedRequest<GarmentCategory, ReadGarmentCategoryViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedGarmentCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper) : GetPagedRequestHandler<GarmentCategory, ReadGarmentCategoryViewModel>(unitOfWork, mapper)
{
}