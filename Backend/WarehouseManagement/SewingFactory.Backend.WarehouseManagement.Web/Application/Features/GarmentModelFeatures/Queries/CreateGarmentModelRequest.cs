using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.Queries;

public record CreateGarmentModelRequest(
    GarmentModelCreateViewModel Model,
    ClaimsPrincipal User)
    : CreateRequest<GarmentModelCreateViewModel, GarmentModel, GarmentModelReadViewModel>(Model, User);

public sealed class CreateGarmentModelHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : CreateRequestHandler<GarmentModelCreateViewModel, GarmentModel, GarmentModelReadViewModel>(unitOfWork, mapper);
