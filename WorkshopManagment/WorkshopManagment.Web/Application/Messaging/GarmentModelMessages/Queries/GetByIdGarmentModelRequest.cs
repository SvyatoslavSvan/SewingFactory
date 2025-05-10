using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.Queries;

public sealed record GetByIdGarmentModelRequest(ClaimsPrincipal User, Guid Id)
    : GetByIdRequest<GarmentModel, DetailsReadGarmentModelViewModel>(User, Id);

public sealed class GetByIdGarmentModelHandler(IUnitOfWork<ApplicationDbContext> unitOfWork, IMapper mapper)
    : GetByIdRequestHandler<GarmentModel, DetailsReadGarmentModelViewModel>(unitOfWork, mapper)
{
}