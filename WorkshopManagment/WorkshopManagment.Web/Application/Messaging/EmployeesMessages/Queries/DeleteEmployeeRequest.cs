using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;

public sealed record DeleteEmployeeRequest(EmployeeDeleteViewModel Model, ClaimsPrincipal User)
    : DeleteRequest<EmployeeDeleteViewModel, Employee>(Model, User);

public sealed class DeleteEmployeeHandler(IUnitOfWork unitOfWork) : DeleteHandler<EmployeeDeleteViewModel, Employee>(unitOfWork)
{
}