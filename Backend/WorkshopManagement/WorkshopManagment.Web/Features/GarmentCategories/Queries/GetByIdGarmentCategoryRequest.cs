using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Queries;

public sealed record GetByIdGarmentCategoryRequest(ClaimsPrincipal User, Guid Id) : GetByIdRequest<GarmentCategory, ReadGarmentCategoryViewModel>(User, Id);

public sealed class GetByIdGarmentCategoryHandler(ApplicationDbContext dbContext, IMapper mapper)
    : GetByIdRequestHandler<GarmentCategory, ReadGarmentCategoryViewModel>(dbContext, mapper)
{
}