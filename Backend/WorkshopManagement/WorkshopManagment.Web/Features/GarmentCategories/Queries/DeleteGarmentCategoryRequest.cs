using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Queries;

public sealed record DeleteGarmentCategoryRequest(DeleteGarmentCategoryViewModel Model, ClaimsPrincipal User) : DeleteRequest<DeleteGarmentCategoryViewModel, GarmentCategory>(Model, User);

public sealed class DeleteGarmentCategoryHandler(IUnitOfWork unitOfWork) : DeleteRequestHandler<DeleteGarmentCategoryViewModel, GarmentCategory>(unitOfWork)
{
}