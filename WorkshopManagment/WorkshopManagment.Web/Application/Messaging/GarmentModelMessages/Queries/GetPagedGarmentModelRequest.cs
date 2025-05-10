using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.Queries;

public sealed record GetPagedGarmentModelRequest(ClaimsPrincipal User, int PageIndex, int PageSize)
    : GetPagedRequest<GarmentModel, ReadGarmentModelViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedGarmentModelHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : GetPagedRequestHandler<GarmentModel, ReadGarmentModelViewModel>(unitOfWork, mapper)
{
}