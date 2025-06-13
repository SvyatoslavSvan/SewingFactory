using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Queries;

public sealed record GetPagedEmployeesRequest(ClaimsPrincipal User, int PageIndex, int PageSize) : GetPagedRequest<Employee, EmployeeReadViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedRequestEmployeesHandler(IUnitOfWork unitOfWork, IMapper mapper) : GetPagedRequestHandler<Employee, EmployeeReadViewModel>(unitOfWork, mapper)
{
}