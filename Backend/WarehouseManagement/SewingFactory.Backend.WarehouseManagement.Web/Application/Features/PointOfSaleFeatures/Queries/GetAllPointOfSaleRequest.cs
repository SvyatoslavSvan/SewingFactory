using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;

public sealed record GetAllPointOfSalesRequest(ClaimsPrincipal User)
    : GetAllRequest<PointOfSale, PointOfSaleReadViewModel>(User);

public sealed class GetAllPointOfSalesHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : GetAllRequestHandler<PointOfSale, PointOfSaleReadViewModel>(unitOfWork, mapper);
