using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Queries;

public record UpdatePointOfSaleRequest(
    PointOfSaleEditViewModel Model,
    ClaimsPrincipal User)
    : UpdateRequest<PointOfSaleEditViewModel, PointOfSale>(Model, User);

public sealed class UpdatePointOfSaleHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : UpdateRequestHandler<PointOfSaleEditViewModel, PointOfSale>(unitOfWork, mapper);
