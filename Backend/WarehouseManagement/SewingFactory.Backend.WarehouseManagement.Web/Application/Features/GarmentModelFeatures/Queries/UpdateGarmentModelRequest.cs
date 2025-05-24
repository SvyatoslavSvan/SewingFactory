using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.Queries;

public record UpdateGarmentModelRequest(
    GarmentModelEditViewModel Model,
    ClaimsPrincipal User)
    : UpdateRequest<GarmentModelEditViewModel, Domain.Entities.GarmentModel>(Model, User);

public sealed class UpdateGarmentModelHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : UpdateRequestHandler<GarmentModelEditViewModel, Domain.Entities.GarmentModel>(unitOfWork, mapper);
