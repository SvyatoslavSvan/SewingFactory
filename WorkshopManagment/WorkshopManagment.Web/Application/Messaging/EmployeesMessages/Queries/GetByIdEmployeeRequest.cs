using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;

public record GetByIdEmployeeRequest(ClaimsPrincipal User, Guid Id) : GetByIdRequest<Employee, EmployeeReadViewModel>(User, Id);

public class GetByIdEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper) : GetByIdHandler<Employee, EmployeeReadViewModel>(unitOfWork, mapper)
{
}