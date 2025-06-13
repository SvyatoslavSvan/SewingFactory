using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Queries;

public sealed record CreateEmployeeRequest(EmployeeCreateViewModel Model, ClaimsPrincipal User) : CreateRequest<EmployeeCreateViewModel, Employee, EmployeeReadViewModel>(Model, User);

public sealed class CreateEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper) : CreateRequestHandler<EmployeeCreateViewModel, Employee, EmployeeReadViewModel>(unitOfWork, mapper)
{
}