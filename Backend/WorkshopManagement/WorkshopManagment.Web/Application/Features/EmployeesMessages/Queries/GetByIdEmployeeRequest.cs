using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.Queries;

public sealed record GetByIdEmployeeRequest(ClaimsPrincipal User, Guid Id) : GetByIdRequest<Employee, EmployeeReadViewModel>(User, Id);

public sealed class GetByIdRequestEmployeeHandler(IUnitOfWork<ApplicationDbContext> unitOfWork, IMapper mapper) : GetByIdRequestHandler<Employee, EmployeeReadViewModel>(unitOfWork, mapper)
{
}