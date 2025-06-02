using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;

public sealed record DeletePointOfSaleRequest(
    PointOfSaleDeleteViewModel Model,
    ClaimsPrincipal User)
    : DeleteRequest<PointOfSaleDeleteViewModel, PointOfSale>(Model, User);

public sealed class DeletePointOfSaleHandler(
    IUnitOfWork unitOfWork)
    : DeleteRequestHandler<PointOfSaleDeleteViewModel, PointOfSale>(unitOfWork);
