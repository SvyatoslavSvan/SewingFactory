using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Queries;

public record DeleteGarmentModelRequest(
    GarmentModelDeleteViewModel Model,
    ClaimsPrincipal User)
    : DeleteRequest<GarmentModelDeleteViewModel, GarmentModel>(Model, User);

public sealed class DeleteGarmentModelHandler(
    IUnitOfWork unitOfWork)
    : DeleteRequestHandler<GarmentModelDeleteViewModel, GarmentModel>(unitOfWork);
