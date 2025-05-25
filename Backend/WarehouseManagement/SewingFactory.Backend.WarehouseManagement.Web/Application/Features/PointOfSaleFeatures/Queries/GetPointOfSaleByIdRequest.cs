using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries
{
    public record GetPointOfSaleByIdRequest(
        ClaimsPrincipal User,
        Guid Id)
        : GetByIdRequest<PointOfSale, PointOfSaleReadViewModel>(User, Id);

    public sealed class GetPointOfSaleByIdHandler(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IMapper mapper)
        : GetByIdRequestHandler<PointOfSale, PointOfSaleReadViewModel>(unitOfWork, mapper);
}
