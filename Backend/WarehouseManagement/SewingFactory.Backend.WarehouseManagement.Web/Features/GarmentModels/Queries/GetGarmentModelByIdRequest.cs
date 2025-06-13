using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Queries;

public record GetGarmentModelByIdRequest(
    ClaimsPrincipal User,
    Guid Id)
    : GetByIdRequest<GarmentModel, GarmentModelReadViewModel>(User, Id);

public sealed class GetGarmentModelByIdHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper)
    : GetByIdRequestHandler<GarmentModel, GarmentModelReadViewModel>(unitOfWork, mapper);
