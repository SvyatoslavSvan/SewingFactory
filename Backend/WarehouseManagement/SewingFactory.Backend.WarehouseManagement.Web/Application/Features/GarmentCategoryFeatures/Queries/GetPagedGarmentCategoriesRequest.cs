using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.Queries;

public record GetPagedGarmentCategoriesRequest(
    ClaimsPrincipal User,
    int PageIndex,
    int PageSize)
    : GetPagedRequest<GarmentCategory, GarmentCategoryReadViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedGarmentCategoriesHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : GetPagedRequestHandler<GarmentCategory, GarmentCategoryReadViewModel>(unitOfWork, mapper);
