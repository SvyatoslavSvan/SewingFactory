using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;

public sealed record GetAllEmployeesRequest(ClaimsPrincipal User) : GetAllRequest<Employee, EmployeeReadViewModel>(User);

public sealed class GetAllRequestEmployeesHandler(IUnitOfWork unitOfWork, IMapper mapper) : GetAllRequestHandler<Employee, EmployeeReadViewModel>(unitOfWork, mapper)
{
}