using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.Queries;

public sealed record GetForUpdateWorkshopDocumentRequest(
    ClaimsPrincipal User) : IRequest<OperationResult<GetForUpdateWorkshopDocumentViewModel>>;

public sealed class GetForUpdateWorkshopDocumentRequestHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper) : IRequestHandler<GetForUpdateWorkshopDocumentRequest, OperationResult<GetForUpdateWorkshopDocumentViewModel>>
{
    public async Task<OperationResult<GetForUpdateWorkshopDocumentViewModel>> Handle(
        GetForUpdateWorkshopDocumentRequest request,
        CancellationToken cancellationToken)
        => OperationResult.CreateResult(new GetForUpdateWorkshopDocumentViewModel
        {
            Employees = mapper.Map<List<EmployeeReadViewModel>>(await unitOfWork.GetRepository<Employee>()
                .GetAllAsync(TrackingType.NoTracking))
        });
}