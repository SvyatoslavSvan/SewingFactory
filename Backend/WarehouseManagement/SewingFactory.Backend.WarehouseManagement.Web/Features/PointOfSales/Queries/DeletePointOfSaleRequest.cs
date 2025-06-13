using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Queries;

public sealed record DeletePointOfSaleRequest(
    PointOfSaleDeleteViewModel Model,
    ClaimsPrincipal User)
    : DeleteRequest<PointOfSaleDeleteViewModel, PointOfSale>(Model, User);

public sealed class DeletePointOfSaleHandler(
    IUnitOfWork unitOfWork)
    : DeleteRequestHandler<PointOfSaleDeleteViewModel, PointOfSale>(unitOfWork);
