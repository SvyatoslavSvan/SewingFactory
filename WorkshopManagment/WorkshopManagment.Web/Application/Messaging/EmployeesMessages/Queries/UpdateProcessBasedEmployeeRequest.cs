using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;

public record UpdateProcessBasedEmployeeRequest(IdentityProcessBasedEmployeeViewModel Model, ClaimsPrincipal User)
    : UpdateRequest<IdentityProcessBasedEmployeeViewModel, ProcessBasedEmployee>(Model, User);

public class UpdateProcessBasedEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper) : UpdateHandler<IdentityProcessBasedEmployeeViewModel, ProcessBasedEmployee>(unitOfWork, mapper)
{
}