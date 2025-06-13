using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.Queries;

public sealed record DeleteDepartmentRequest(DeleteDepartmentViewModel Model, ClaimsPrincipal User)
    : DeleteRequest<DeleteDepartmentViewModel, Department>(Model, User);

public sealed class DeleteDepartmentHandler(IUnitOfWork unitOfWork)
    : DeleteRequestHandler<DeleteDepartmentViewModel, Department>(unitOfWork);