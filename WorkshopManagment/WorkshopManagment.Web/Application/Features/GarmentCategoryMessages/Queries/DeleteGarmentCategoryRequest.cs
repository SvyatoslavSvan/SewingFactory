using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.Queries;

public sealed record DeleteGarmentCategoryRequest(DeleteGarmentCategoryViewModel Model, ClaimsPrincipal User) : DeleteRequest<DeleteGarmentCategoryViewModel, GarmentCategory>(Model, User);

public sealed class DeleteGarmentCategoryHandler(IUnitOfWork unitOfWork) : DeleteRequestHandler<DeleteGarmentCategoryViewModel, GarmentCategory>(unitOfWork)
{
}