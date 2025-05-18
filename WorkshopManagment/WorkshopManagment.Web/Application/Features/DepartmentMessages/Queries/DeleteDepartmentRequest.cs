using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.Queries;

public sealed record DeleteDepartmentRequest(DeleteDepartmentViewModel Model, ClaimsPrincipal User)
    : DeleteRequest<DeleteDepartmentViewModel, Department>(Model, User);

public sealed class DeleteDepartmentHandler(IUnitOfWork unitOfWork)
    : DeleteRequestHandler<DeleteDepartmentViewModel, Department>(unitOfWork);