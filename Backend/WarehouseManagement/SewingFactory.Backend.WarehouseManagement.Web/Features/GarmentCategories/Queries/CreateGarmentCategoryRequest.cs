using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Queries;

public record CreateGarmentCategoryRequest(
    GarmentCategoryCreateViewModel Model,
    ClaimsPrincipal User)
    : CreateRequest<GarmentCategoryCreateViewModel, GarmentCategory, GarmentCategoryReadViewModel>(Model, User);

public sealed class CreateGarmentCategoryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : CreateRequestHandler<GarmentCategoryCreateViewModel, GarmentCategory, GarmentCategoryReadViewModel>(unitOfWork, mapper);
