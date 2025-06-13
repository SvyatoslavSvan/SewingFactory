using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Interfaces;
using SewingFactory.Common.Domain.ValueObjects;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed record AllSalesForGarmentModelReportRequest(ClaimsPrincipal User, DateRange dateRange, Guid GarmentModelId) : IRequest<IResult>;

public sealed class AllSalesForGarmentModelReportRequestHandler(IAllSalesForGarmentModelReportProvider provider,IUnitOfWork unitOfWork) : IRequestHandler<AllSalesForGarmentModelReportRequest, IResult>
{
    public async Task<IResult> Handle(AllSalesForGarmentModelReportRequest request, CancellationToken cancellationToken)
    {
        var sales = await unitOfWork.GetRepository<SaleOperation>().GetAllAsync(
            predicate: saleOperation =>
                saleOperation.Date >= request.dateRange.Start &&
                saleOperation.Date <= request.dateRange.End &&
                saleOperation.StockItem.GarmentModel.Id == request.GarmentModelId,
            include: queryable => queryable
                .Include(saleOperation => saleOperation.StockItem)
                .ThenInclude(stockItem => stockItem.GarmentModel)
                .Include(x => x.Owner),
            trackingType: TrackingType.NoTracking);
        
        var report = provider.Build(sales, request.dateRange);

        var fileName = $"Продажі по моделі {sales.First().StockItem.GarmentModel.Name} : {request.dateRange}.xlsx";

        return Results.File(
            report,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }
}
