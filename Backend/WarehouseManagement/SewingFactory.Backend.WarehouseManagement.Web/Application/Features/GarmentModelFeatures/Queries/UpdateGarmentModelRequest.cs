using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.Queries;

public record UpdateGarmentModelRequest(
    GarmentModelEditViewModel Model,
    ClaimsPrincipal User)
    : UpdateRequest<GarmentModelEditViewModel, GarmentModel>(Model, User);

public sealed class UpdateGarmentModelHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : UpdateRequestHandler<GarmentModelEditViewModel, GarmentModel>(unitOfWork, mapper);
