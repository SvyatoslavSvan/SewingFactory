using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentCategoryMessages.Queries;

public sealed record DeleteGarmentCategoryRequest(DeleteGarmentCategoryViewModel Model, ClaimsPrincipal User) : DeleteRequest<DeleteGarmentCategoryViewModel, GarmentCategory>(Model, User);

public sealed class DeleteGarmentCategoryHandler(IUnitOfWork unitOfWork) : DeleteRequestHandler<DeleteGarmentCategoryViewModel, GarmentCategory>(unitOfWork)
{
}