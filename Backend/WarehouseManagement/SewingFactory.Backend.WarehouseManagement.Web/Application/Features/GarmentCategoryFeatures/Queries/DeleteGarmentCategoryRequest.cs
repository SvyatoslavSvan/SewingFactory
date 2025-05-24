using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.Queries;

public record DeleteGarmentCategoryRequest(
    GarmentCategoryDeleteViewModel Model,
    ClaimsPrincipal User)
    : DeleteRequest<GarmentCategoryDeleteViewModel, GarmentCategory>(Model, User);

public sealed class DeleteGarmentCategoryHandler(
    IUnitOfWork unitOfWork)
    : DeleteRequestHandler<GarmentCategoryDeleteViewModel, GarmentCategory>(unitOfWork);
