using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Queries;

public record GetPagedPointOfSalesRequest(
    ClaimsPrincipal User,
    int PageIndex,
    int PageSize)
    : GetPagedRequest<PointOfSale, PointOfSaleReadViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedPointOfSalesHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : GetPagedRequestHandler<PointOfSale, PointOfSaleReadViewModel>(unitOfWork, mapper);
