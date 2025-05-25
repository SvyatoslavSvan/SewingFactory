using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries
{
    public record GetPagedPointOfSalesRequest(
        ClaimsPrincipal User,
        int PageIndex,
        int PageSize)
        : GetPagedRequest<PointOfSale, PointOfSaleReadViewModel>(User, PageIndex, PageSize);

    public sealed class GetPagedPointOfSalesHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : GetPagedRequestHandler<PointOfSale, PointOfSaleReadViewModel>(unitOfWork, mapper);
}
