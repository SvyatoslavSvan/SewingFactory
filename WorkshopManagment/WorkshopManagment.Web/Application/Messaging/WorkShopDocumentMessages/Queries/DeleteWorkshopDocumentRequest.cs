using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Document;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkshopDocumentMessages.Queries;

public sealed record DeleteWorkshopDocumentRequest(
    DeleteWorkshopDocumentViewModel Model,
    ClaimsPrincipal User)
    : DeleteRequest<DeleteWorkshopDocumentViewModel, WorkshopDocument>(Model, User);

public sealed class DeleteWorkshopDocumentHandler(IUnitOfWork unitOfWork)
    : DeleteRequestHandler<DeleteWorkshopDocumentViewModel, WorkshopDocument>(unitOfWork);