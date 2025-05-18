using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.Queries;

public sealed record CreateEmployeeRequest(EmployeeCreateViewModel Model, ClaimsPrincipal User) : CreateRequest<EmployeeCreateViewModel, Employee, EmployeeReadViewModel>(Model, User);

public sealed class CreateEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper) : CreateRequestHandler<EmployeeCreateViewModel, Employee, EmployeeReadViewModel>(unitOfWork, mapper)
{
}