using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;

public sealed record UpdateProcessBasedEmployeeRequest(EmployeeUpdateViewModel Model, ClaimsPrincipal User)
    : UpdateRequest<EmployeeUpdateViewModel, Employee>(Model, User);

public sealed class UpdateRequestProcessBasedEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper) : UpdateRequestHandler<EmployeeUpdateViewModel, Employee>(unitOfWork, mapper)
{
}