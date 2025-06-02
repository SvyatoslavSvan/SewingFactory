using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.Queries;

public record GetAllGarmentModelsRequest(ClaimsPrincipal User)
    : GetAllRequest<GarmentModel, GarmentModelReadViewModel>(User);

public sealed class GetAllGarmentModelsHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper)
    : GetAllRequestHandler<GarmentModel, GarmentModelReadViewModel>(unitOfWork, mapper);
