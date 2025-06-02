using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.Queries;

public record GetAllGarmentCategoriesRequest(ClaimsPrincipal User)
    : GetAllRequest<GarmentCategory, GarmentCategoryReadViewModel>(User);

public sealed class GetAllGarmentCategoriesHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper)
    : GetAllRequestHandler<GarmentCategory, GarmentCategoryReadViewModel>(unitOfWork, mapper);
