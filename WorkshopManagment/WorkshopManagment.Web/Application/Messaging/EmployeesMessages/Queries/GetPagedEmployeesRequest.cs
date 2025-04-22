using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;

public record GetPagedEmployeesRequest(ClaimsPrincipal User, int PageIndex, int PageSize) : GetPagedRequest<Employee>(User, PageIndex, PageSize);

public class GetPagedEmployeesHandler(IUnitOfWork unitOfWork) : GetPagedHandler<Employee>(unitOfWork)
{
}