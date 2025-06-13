using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Queries;

public sealed record UpdateProcessBasedEmployeeRequest(EmployeeUpdateViewModel Model, ClaimsPrincipal User)
    : UpdateRequest<EmployeeUpdateViewModel, Employee>(Model, User);

public sealed class UpdateRequestProcessBasedEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper) : UpdateRequestHandler<EmployeeUpdateViewModel, Employee>(unitOfWork, mapper)
{
}