using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentCategoryMessages.Queries;

public sealed record GetAllGarmentCategoryRequest(ClaimsPrincipal User) : GetAllRequest<GarmentCategory, ReadGarmentCategoryViewModel>(User);

public sealed class GetAllGarmentCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper) : GetAllRequestHandler<GarmentCategory, ReadGarmentCategoryViewModel>(unitOfWork, mapper)
{
}