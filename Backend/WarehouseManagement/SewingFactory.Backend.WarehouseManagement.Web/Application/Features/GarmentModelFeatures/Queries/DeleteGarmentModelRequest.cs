using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.Queries;

public record DeleteGarmentModelRequest(
    GarmentModelDeleteViewModel Model,
    ClaimsPrincipal User)
    : DeleteRequest<GarmentModelDeleteViewModel, GarmentModel>(Model, User);

public sealed class DeleteGarmentModelHandler(
    IUnitOfWork unitOfWork)
    : DeleteRequestHandler<GarmentModelDeleteViewModel, GarmentModel>(unitOfWork);
