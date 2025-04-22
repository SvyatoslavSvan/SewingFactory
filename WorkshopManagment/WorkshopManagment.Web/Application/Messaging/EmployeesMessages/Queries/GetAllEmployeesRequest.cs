using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries
{
    public record GetAllEmployeesRequest(ClaimsPrincipal User) : GetAllRequest<Employee>(User);

    public class GetAllEmployeesHandler(IUnitOfWork unitOfWork) : GetAllHandler<Employee>(unitOfWork)
    {  }
}
