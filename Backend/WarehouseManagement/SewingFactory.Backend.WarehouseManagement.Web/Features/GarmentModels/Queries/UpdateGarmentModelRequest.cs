using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Queries;

public record UpdateGarmentModelRequest(
    GarmentModelEditViewModel Model,
    ClaimsPrincipal User)
    : UpdateRequest<GarmentModelEditViewModel, GarmentModel>(Model, User);

public sealed class UpdateGarmentModelHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : UpdateRequestHandler<GarmentModelEditViewModel, GarmentModel>(unitOfWork, mapper);
