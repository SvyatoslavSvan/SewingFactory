using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.Queries;

public record GetGarmentCategoryByIdRequest(
    ClaimsPrincipal User,
    Guid Id)
    : GetByIdRequest<GarmentCategory, GarmentCategoryReadViewModel>(User, Id);

public sealed class GetGarmentCategoryByIdHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper)
    : GetByIdRequestHandler<GarmentCategory, GarmentCategoryReadViewModel>(unitOfWork, mapper);
