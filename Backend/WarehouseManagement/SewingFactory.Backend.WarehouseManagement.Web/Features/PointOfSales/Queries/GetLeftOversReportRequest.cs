﻿using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Interfaces;
using System.Security.Claims;
using TimeZoneConverter;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Queries;

public sealed record GetLeftOversReportRequest(
    ClaimsPrincipal User,
    Guid PointOfSaleId) : IRequest<IResult>;

public class GetLeftOversReportRequestHandler(
    IUnitOfWork unitOfWork,
    ILeftoverReportProvider reportProvider) : IRequestHandler<GetLeftOversReportRequest, IResult>
{
    public async Task<IResult> Handle(
        GetLeftOversReportRequest request,
        CancellationToken cancellationToken)
    {
        var pointOfSale = await unitOfWork.GetRepository<PointOfSale>()
            .GetFirstOrDefaultAsync(predicate: pointOfSale => pointOfSale.Id == request.PointOfSaleId,
                include: queryable => queryable.Include(navigationPropertyPath: pointOfSale => pointOfSale.StockItems)
                    .ThenInclude(navigationPropertyPath: stockItem => stockItem.GarmentModel)
                    .ThenInclude(navigationPropertyPath: garmentModel => garmentModel.Category),
                trackingType: TrackingType.NoTrackingWithIdentityResolution);

        if (pointOfSale is null)
        {
            return Results.NotFound($"Point of Sale with ID {request.PointOfSaleId} not found.");
        }

        var reportBytes = reportProvider.Build(pointOfSale);
        var fileName = $"Звіт по залишках за: {DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TZConvert.GetTimeZoneInfo("Europe/Kyiv")))}.xlsx";

        return Results.File(
            reportBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName
        );
    }
}
