using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Queries;

public sealed record GetAllEmployeesRequest(ClaimsPrincipal User) : GetAllRequest<Employee, EmployeeReadViewModel>(User);

public sealed class GetAllRequestEmployeesHandler(IUnitOfWork unitOfWork, IMapper mapper) : GetAllRequestHandler<Employee, EmployeeReadViewModel>(unitOfWork, mapper)
{
}