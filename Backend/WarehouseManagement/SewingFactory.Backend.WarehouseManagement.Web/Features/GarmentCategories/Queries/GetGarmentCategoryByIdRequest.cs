using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Queries;

public record GetGarmentCategoryByIdRequest(
    ClaimsPrincipal User,
    Guid Id)
    : GetByIdRequest<GarmentCategory, GarmentCategoryReadViewModel>(User, Id);

public sealed class GetGarmentCategoryByIdHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper)
    : GetByIdRequestHandler<GarmentCategory, GarmentCategoryReadViewModel>(unitOfWork, mapper);
