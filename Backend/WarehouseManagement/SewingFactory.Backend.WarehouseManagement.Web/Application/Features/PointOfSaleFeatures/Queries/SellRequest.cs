using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Operations;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;
using Operation = Calabonga.OperationResults.Operation;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;

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
            .GetFirstOrDefaultAsync(predicate: x => x.Id == request.Model.PointOfSaleId,
                include: q => q.Include(navigationPropertyPath: pointOfSale => pointOfSale.StockItems).ThenInclude(navigationPropertyPath: x => x.GarmentModel)
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
