using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.Queries;

public record GetPagedGarmentModelsRequest(
    ClaimsPrincipal User,
    int PageIndex,
    int PageSize)
    : GetPagedRequest<GarmentModel, GarmentModelReadViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedGarmentModelsHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : GetPagedRequestHandler<GarmentModel, GarmentModelReadViewModel>(unitOfWork, mapper);
