using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Queries;

public sealed record GetByIdEmployeeRequest(ClaimsPrincipal User, Guid Id) : GetByIdRequest<Employee, EmployeeReadViewModel>(User, Id);

public sealed class GetByIdRequestEmployeeHandler(
    ApplicationDbContext dbContext,
    IMapper mapper) : GetByIdRequestHandler<Employee, EmployeeReadViewModel>(dbContext,
    mapper)
{
}