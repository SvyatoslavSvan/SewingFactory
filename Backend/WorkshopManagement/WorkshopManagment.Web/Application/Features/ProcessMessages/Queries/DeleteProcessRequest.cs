using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.Queries;

public sealed record DeleteProcessRequest(DeleteProcessViewModel Model, ClaimsPrincipal User)
    : DeleteRequest<DeleteProcessViewModel, Process>(Model, User);

public sealed class DeleteProcessHandler(IUnitOfWork unitOfWork)
    : DeleteRequestHandler<DeleteProcessViewModel, Process>(unitOfWork)
{
}