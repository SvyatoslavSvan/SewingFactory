using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.Queries;

public sealed record GetAllEmployeesRequest(ClaimsPrincipal User) : GetAllRequest<Employee, EmployeeReadViewModel>(User);

public sealed class GetAllRequestEmployeesHandler(IUnitOfWork unitOfWork, IMapper mapper) : GetAllRequestHandler<Employee, EmployeeReadViewModel>(unitOfWork, mapper)
{
}