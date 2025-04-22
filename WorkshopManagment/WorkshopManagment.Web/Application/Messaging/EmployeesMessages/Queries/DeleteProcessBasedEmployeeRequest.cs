using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;

public record DeleteProcessBasedEmployeeRequest(DeleteProcessBasedEmployeeViewModel Model, ClaimsPrincipal User) : DeleteRequest<DeleteProcessBasedEmployeeViewModel, ProcessBasedEmployee>(Model, User);

public class DeleteProcessBasedEmployeeHandler(IUnitOfWork unitOfWork) : DeleteHandler<DeleteProcessBasedEmployeeViewModel, ProcessBasedEmployee>(unitOfWork)
{
   
}