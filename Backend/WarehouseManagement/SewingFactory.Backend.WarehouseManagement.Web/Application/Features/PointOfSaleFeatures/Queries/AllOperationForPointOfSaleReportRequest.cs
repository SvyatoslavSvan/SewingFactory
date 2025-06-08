using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Interfaces;
using SewingFactory.Common.Domain.ValueObjects;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;

public sealed record AllOperationForPointOfSaleReportRequest(ClaimsPrincipal User, Guid PointOfSaleId, DateRange dateRange) : IRequest<IResult>;

public sealed class AllOperationForPointOfSaleReportRequestHandler(IAllOperationForPointOfSaleReportProvider provider, IUnitOfWork unitOfWork) : IRequestHandler<AllOperationForPointOfSaleReportRequest, IResult>
{
    public async Task<IResult> Handle(AllOperationForPointOfSaleReportRequest request, CancellationToken cancellationToken)
    {
        var pointOfSale = await unitOfWork.GetRepository<PointOfSale>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == request.PointOfSaleId,
                include: q => q.Include(x => x.Operations.Where(o => o.Date >= request.dateRange.Start && o.Date <= request.dateRange.End))
                    .Include(x => x.Operations)
                    .ThenInclude(x => (x as InternalTransferOperation)!.Receiver));

        if (pointOfSale is null)
        {
            return Results.NotFound($"Point of Sale with ID {request.PointOfSaleId} not found.");
        }
        
        var report = provider.Build(pointOfSale, request.dateRange);
        
        var fileName = $"Реєстр по торговій точці {pointOfSale.Name} за: {request.dateRange}.xlsx";

        return Results.File(
            report,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }
}
