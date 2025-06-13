using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Queries;

public record GetAllGarmentCategoriesRequest(ClaimsPrincipal User)
    : GetAllRequest<GarmentCategory, GarmentCategoryReadViewModel>(User);

public sealed class GetAllGarmentCategoriesHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper)
    : GetAllRequestHandler<GarmentCategory, GarmentCategoryReadViewModel>(unitOfWork, mapper);
