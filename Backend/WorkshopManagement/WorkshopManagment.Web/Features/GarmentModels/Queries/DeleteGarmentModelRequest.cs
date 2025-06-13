using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Queries;

public sealed record DeleteGarmentModelRequest(DeleteGarmentModelViewModel Model, ClaimsPrincipal User)
    : DeleteRequest<DeleteGarmentModelViewModel, GarmentModel>(Model, User);

public sealed class DeleteGarmentModelHandler(IUnitOfWork unitOfWork)
    : DeleteRequestHandler<DeleteGarmentModelViewModel, GarmentModel>(unitOfWork)
{
}