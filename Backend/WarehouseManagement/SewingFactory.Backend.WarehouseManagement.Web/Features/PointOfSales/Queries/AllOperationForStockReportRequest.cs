using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Interfaces;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Queries;

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
                selector: pointOfSale => new
                {
                    PointOfSale = pointOfSale,
                    GarmentModel = pointOfSale.Operations
                        .Where(operation => operation.StockItem.GarmentModel.Id == request.GarmentModelId)
                        .OrderByDescending(operation => operation.Date) 
                        .Select(operation => operation.StockItem.GarmentModel)
                        .FirstOrDefault()
                },
                predicate: pointOfSale => pointOfSale.Id == request.PointOfSaleId,
                include: queryable => queryable
                    .Include(pointOfSale => pointOfSale.Operations
                        .Where(operation => operation.Date >= request.Range.Start
                                    && operation.Date <= request.Range.End))
                    .ThenInclude(operation => operation.StockItem)
                    .ThenInclude(stockItem => stockItem.GarmentModel)
                    .Include(pointOfSale => pointOfSale.Operations)
                    .ThenInclude(operation => (operation as InternalTransferOperation)!.Receiver),
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
