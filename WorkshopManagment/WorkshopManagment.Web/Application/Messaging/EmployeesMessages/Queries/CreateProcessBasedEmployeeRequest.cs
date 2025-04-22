using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;

public record CreateProcessBasedEmployeeRequest(ProcessBasedEmployeeViewModel Model, ClaimsPrincipal User)
    : CreateRequest<ProcessBasedEmployeeViewModel, ProcessBasedEmployee, IdentityProcessBasedEmployeeViewModel>(Model, User);

public class CreateProcessBasedEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : CreateRequestHandler<ProcessBasedEmployeeViewModel, ProcessBasedEmployee, IdentityProcessBasedEmployeeViewModel>(unitOfWork, mapper)
{
}