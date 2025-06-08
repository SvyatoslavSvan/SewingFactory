using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Interfaces;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;
using System.Security.Claims;
using TimeZoneConverter;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;

public sealed record AllOperationForStockReportRequest(
    ClaimsPrincipal User,
    DateRange Range,
    Guid PointOfSaleId,
    Guid GarmentModelId) : IRequest<IResult>;

public sealed class AllOperationForStockReportRequestHandler(
    IUnitOfWork unitOfWork,
    IAllOperationForStockReportProvider provider) : IRequestHandler<AllOperationForStockReportRequest, IResult>
{
    public async Task<IResult> Handle(
        AllOperationForStockReportRequest request,
        CancellationToken cancellationToken)
    {
        var data = await unitOfWork.GetRepository<PointOfSale>()
            .GetFirstOrDefaultAsync(
                selector: pos => new
                {
                    PointOfSale = pos,
                    GarmentModel = pos.Operations
                        .Where(op => op.StockItem.GarmentModel.Id == request.GarmentModelId)
                        .OrderByDescending(op => op.Date) 
                        .Select(op => op.StockItem.GarmentModel)
                        .FirstOrDefault()
                },
                predicate: pos => pos.Id == request.PointOfSaleId,
                include: q => q
                    .Include(p => p.Operations
                        .Where(o => o.Date >= request.Range.Start
                                    && o.Date <= request.Range.End))
                    .ThenInclude(o => o.StockItem)
                    .ThenInclude(si => si.GarmentModel)
                    .Include(p => p.Operations)
                    .ThenInclude(o => (o as InternalTransferOperation)!.Receiver),
                trackingType: TrackingType.NoTrackingWithIdentityResolution);


        var pointOfSale = data?.PointOfSale ??
                          throw new SewingFactoryArgumentException(nameof(request.PointOfSaleId),
                              $"Point of Sale with {request.PointOfSaleId} not found.");

        var garmentModel = data.GarmentModel ??
                           throw new SewingFactoryArgumentException(nameof(request.PointOfSaleId),
                               $"GarmentModel with {request.GarmentModelId} not found.");

        var reportBytes = provider.Build(pointOfSale,
            garmentModel,
            request.Range);

        var fileName = $"Реєстр по товару {garmentModel.Name} у тт {pointOfSale.Name} за: {request.Range}.xlsx";

        return Results.File(
            reportBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }
}
