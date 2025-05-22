using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.Queries;

public sealed record GetAllGarmentModelRequest(ClaimsPrincipal User)
    : GetAllRequest<GarmentModel, ReadGarmentModelViewModel>(User);

public sealed class GetAllGarmentModelHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : GetAllRequestHandler<GarmentModel, ReadGarmentModelViewModel>(unitOfWork, mapper)
{
}