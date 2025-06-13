using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Queries;

public record UpdateGarmentCategoryRequest(
    GarmentCategoryEditViewModel Model,
    ClaimsPrincipal User)
    : UpdateRequest<GarmentCategoryEditViewModel, GarmentCategory>(Model, User);

public sealed class UpdateGarmentCategoryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : UpdateRequestHandler<GarmentCategoryEditViewModel, GarmentCategory>(unitOfWork, mapper);
