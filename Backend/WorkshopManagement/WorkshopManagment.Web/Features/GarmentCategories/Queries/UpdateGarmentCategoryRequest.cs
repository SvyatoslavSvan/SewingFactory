using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Queries;

public sealed record UpdateGarmentCategoryRequest(UpdateGarmentCategoryViewModel Model, ClaimsPrincipal User) : UpdateRequest<UpdateGarmentCategoryViewModel, GarmentCategory>(Model, User);

public sealed class UpdateRequestGarmentCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper) : UpdateRequestHandler<UpdateGarmentCategoryViewModel, GarmentCategory>(unitOfWork, mapper)
{
}