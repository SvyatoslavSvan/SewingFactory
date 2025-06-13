using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Queries;

public sealed record CreateGarmentCategoryRequest(CreateGarmentCategoryViewModel Model, ClaimsPrincipal User)
    : CreateRequest<CreateGarmentCategoryViewModel, GarmentCategory, ReadGarmentCategoryViewModel>(Model, User);

public sealed class CreateGarmentCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : CreateRequestHandler<CreateGarmentCategoryViewModel, GarmentCategory, ReadGarmentCategoryViewModel>(unitOfWork, mapper)
{
}