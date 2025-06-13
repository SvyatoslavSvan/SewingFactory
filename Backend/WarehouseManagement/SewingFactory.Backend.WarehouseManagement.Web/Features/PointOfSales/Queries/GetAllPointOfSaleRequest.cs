using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Queries;

public sealed record GetAllPointOfSalesRequest(ClaimsPrincipal User)
    : GetAllRequest<PointOfSale, PointOfSaleReadViewModel>(User);

public sealed class GetAllPointOfSalesHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : GetAllRequestHandler<PointOfSale, PointOfSaleReadViewModel>(unitOfWork, mapper);
