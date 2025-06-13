using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Queries;

public record DeleteGarmentCategoryRequest(
    GarmentCategoryDeleteViewModel Model,
    ClaimsPrincipal User)
    : DeleteRequest<GarmentCategoryDeleteViewModel, GarmentCategory>(Model, User);

public sealed class DeleteGarmentCategoryHandler(
    IUnitOfWork unitOfWork)
    : DeleteRequestHandler<GarmentCategoryDeleteViewModel, GarmentCategory>(unitOfWork);
