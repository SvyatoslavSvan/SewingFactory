using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Queries;

public sealed record CreateGarmentModelRequest(CreateGarmentModelViewModel Model, ClaimsPrincipal User)
    : CreateRequest<CreateGarmentModelViewModel, GarmentModel, DetailsReadGarmentModelViewModel>(Model, User);

public sealed class CreateGarmentModelHandler(IUnitOfWork<ApplicationDbContext> unitOfWork, IMapper mapper)
    : CreateRequestHandler<CreateGarmentModelViewModel, GarmentModel, DetailsReadGarmentModelViewModel>(unitOfWork, mapper)
{
}