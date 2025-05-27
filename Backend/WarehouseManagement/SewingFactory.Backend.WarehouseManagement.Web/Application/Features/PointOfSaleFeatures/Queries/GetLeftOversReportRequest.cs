using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides;
using System.Security.Claims;
using TimeZoneConverter;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries
{
    public record GetLeftOversReportRequest(
        ClaimsPrincipal User,
        Guid PointOfSaleId) : IRequest<IResult>;

    public class GetLeftOversReportRequestHandler(
        IUnitOfWork unitOfWork, ILeftoverReportProvider reportProvider) : IRequestHandler<GetLeftOversReportRequest, IResult>
    {
        public async Task<IResult> Handle(
            GetLeftOversReportRequest request,
            CancellationToken cancellationToken)
        {
            var pointOfSale = await unitOfWork.GetRepository<PointOfSale>()
                .GetFirstOrDefaultAsync(predicate: x => x.Id == request.PointOfSaleId,
                    include: queryable => queryable.Include(pointOfSale => pointOfSale.StockItems)
                        .ThenInclude(stockItem => stockItem.GarmentModel)
                        .ThenInclude(garmentModel => garmentModel.Category), 
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
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: fileName
            );
            
        }
    }
}
