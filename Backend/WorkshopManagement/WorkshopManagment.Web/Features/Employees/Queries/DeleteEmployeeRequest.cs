using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Queries;

public sealed record DeleteEmployeeRequest(EmployeeDeleteViewModel Model, ClaimsPrincipal User)
    : DeleteRequest<EmployeeDeleteViewModel, Employee>(Model, User);

public sealed class DeleteRequestEmployeeHandler(IUnitOfWork unitOfWork) : DeleteRequestHandler<EmployeeDeleteViewModel, Employee>(unitOfWork)
{
}