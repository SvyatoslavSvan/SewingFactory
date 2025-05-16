using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.Queries;

public sealed record DeleteDepartmentRequest(DeleteDepartmentViewModel Model, ClaimsPrincipal User)
    : DeleteRequest<DeleteDepartmentViewModel, Department>(Model, User);

public sealed class DeleteDepartmentHandler(IUnitOfWork unitOfWork)
    : DeleteRequestHandler<DeleteDepartmentViewModel, Department>(unitOfWork);