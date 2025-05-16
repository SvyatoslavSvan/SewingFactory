using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Document;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.Queries;

public record GetForUpdateWorkshopDocumentRequest(
    ClaimsPrincipal User) : IRequest<OperationResult<GetForUpdateWorkshopDocumentViewModel>>;

public class GetForUpdateWorkshopDocumentRequestHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper) : IRequestHandler<GetForUpdateWorkshopDocumentRequest, OperationResult<GetForUpdateWorkshopDocumentViewModel>>
{
    public async Task<OperationResult<GetForUpdateWorkshopDocumentViewModel>> Handle(
        GetForUpdateWorkshopDocumentRequest request,
        CancellationToken cancellationToken)
        => OperationResult.CreateResult(new GetForUpdateWorkshopDocumentViewModel
        {
            Employees = mapper.Map<List<EmployeeReadViewModel>>(await unitOfWork.GetRepository<Employee>()
                .GetAllAsync(true))
        });
}