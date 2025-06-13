using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Queries;

public sealed record CreatePointOfSaleRequest(
    PointOfSaleCreateViewModel Model,
    ClaimsPrincipal User)
    : CreateRequest<PointOfSaleCreateViewModel, PointOfSale, PointOfSaleDetailsReadViewModel>(Model, User);

public sealed class CreatePointOfSaleHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : CreateRequestHandler<PointOfSaleCreateViewModel, PointOfSale, PointOfSaleDetailsReadViewModel>(unitOfWork, mapper);
