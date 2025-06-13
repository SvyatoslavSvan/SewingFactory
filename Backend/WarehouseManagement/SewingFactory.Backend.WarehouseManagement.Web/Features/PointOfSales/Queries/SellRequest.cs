using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.Operations;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;
using Operation = Calabonga.OperationResults.Operation;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Queries;

public sealed record SellRequest(
    CreateOperationViewModel Model,
    ClaimsPrincipal User) : IRequest<OperationEmpty<SewingFactoryNotFoundException, Exception>>;

public sealed class SellRequestRequestHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<SellRequest, OperationEmpty<SewingFactoryNotFoundException, Exception>>
{
    public async Task<OperationEmpty<SewingFactoryNotFoundException, Exception>> Handle(
        SellRequest request,
        CancellationToken cancellationToken)
    {
        var pointOfSale = await unitOfWork.GetRepository<PointOfSale>()
            .GetFirstOrDefaultAsync(predicate: pointOfSale => pointOfSale.Id == request.Model.PointOfSaleId,
                include: queryable => queryable.Include(navigationPropertyPath: pointOfSale => pointOfSale.StockItems).ThenInclude(navigationPropertyPath: stockItem => stockItem.GarmentModel)
                    .Include(navigationPropertyPath: pointOfSale => pointOfSale.Operations), trackingType: TrackingType.Tracking);

        if (pointOfSale is null)
        {
            return Operation.Error(new SewingFactoryNotFoundException($"{nameof(PointOfSale)} with {request.Model.PointOfSaleId} was not found"));
        }

        pointOfSale.Sell(request.Model.GarmentModelId, request.Model.Quantity, request.Model.Date);

        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.Result.Ok)
        {
            return Operation.Error(unitOfWork.Result.Exception ?? new SewingFactoryDatabaseSaveException($"{nameof(PointOfSale)} error while saving {nameof(PointOfSale.AddStockItem)} operation"));
        }

        return Operation.Result();
    }
}
