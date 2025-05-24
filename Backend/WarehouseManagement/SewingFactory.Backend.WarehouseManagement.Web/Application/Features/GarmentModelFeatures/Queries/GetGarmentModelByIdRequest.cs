using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.Queries;

public record GetGarmentModelByIdRequest(
    ClaimsPrincipal User,
    Guid Id)
    : GetByIdRequest<GarmentModel, GarmentModelReadViewModel>(User, Id);

public sealed class GetGarmentModelByIdHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper)
    : GetByIdRequestHandler<GarmentModel, GarmentModelReadViewModel>(unitOfWork, mapper);
