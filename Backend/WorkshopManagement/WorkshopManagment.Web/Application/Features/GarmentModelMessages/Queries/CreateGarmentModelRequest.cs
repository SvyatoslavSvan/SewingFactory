using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.Queries;

public sealed record CreateGarmentModelRequest(CreateGarmentModelViewModel Model, ClaimsPrincipal User)
    : CreateRequest<CreateGarmentModelViewModel, GarmentModel, DetailsReadGarmentModelViewModel>(Model, User);

public sealed class CreateGarmentModelHandler(IUnitOfWork<ApplicationDbContext> unitOfWork, IMapper mapper)
    : CreateRequestHandler<CreateGarmentModelViewModel, GarmentModel, DetailsReadGarmentModelViewModel>(unitOfWork, mapper)
{
}