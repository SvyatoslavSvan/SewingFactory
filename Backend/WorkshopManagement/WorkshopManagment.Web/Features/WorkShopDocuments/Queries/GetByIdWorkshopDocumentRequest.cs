using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.Queries;

public sealed record GetByIdWorkshopDocumentRequest(
    ClaimsPrincipal User,
    Guid Id)
    : GetByIdRequest<WorkshopDocument, DetailsReadWorkshopDocumentViewModel>(User, Id);

public sealed class GetByIdWorkshopDocumentHandler(
    ApplicationDbContext dbContext,
    IMapper mapper) : GetByIdRequestHandler<WorkshopDocument, DetailsReadWorkshopDocumentViewModel>(dbContext, mapper)
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public override async Task<OperationResult<DetailsReadWorkshopDocumentViewModel>> Handle(
        GetByIdRequest<WorkshopDocument, DetailsReadWorkshopDocumentViewModel> request, CancellationToken cancellationToken)
    {
        var result = OperationResult.CreateResult<DetailsReadWorkshopDocumentViewModel>();
        var document = await _dbContext.WorkshopDocuments.Include(x => x.Tasks)
            .ThenInclude(x => x.EmployeeTaskRepeats)
            .ThenInclude(x => x.WorkShopEmployee)
            .Include(x => x.Tasks)
            .ThenInclude(x => x.Process)
            .FirstOrDefaultAsync(x => x.Id == request.Id,
                cancellationToken: cancellationToken);
        
        if (document is null)
        {
            result.AddError($"The entity {nameof(WorkshopDocument)} was not found with key {request.Id}");

            return result;
        }

        result.Result = _mapper.Map<DetailsReadWorkshopDocumentViewModel>(document);

        return result;
    }
}