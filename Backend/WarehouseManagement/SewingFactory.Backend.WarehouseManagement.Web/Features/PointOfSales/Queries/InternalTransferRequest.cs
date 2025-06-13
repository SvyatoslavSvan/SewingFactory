using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.Operations;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;
using Operation = Calabonga.OperationResults.Operation;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Queries;

public sealed record InternalTransferRequest(
    CreateInternalTransferViewModel Model,
    ClaimsPrincipal User
) : IRequest<OperationEmpty<SewingFactoryNotFoundException, Exception>>;

public sealed class InternalTransferRequestHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork
) : IRequestHandler<InternalTransferRequest, OperationEmpty<SewingFactoryNotFoundException, Exception>>
{
    public async Task<OperationEmpty<SewingFactoryNotFoundException, Exception>> Handle(
        InternalTransferRequest request,
        CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;
        var pair = await context.Set<PointOfSale>()
            .Include(navigationPropertyPath: pointOfSale => pointOfSale.StockItems)
            .ThenInclude(navigationPropertyPath: stockItem => stockItem.GarmentModel)
            .Include(navigationPropertyPath: pointOfSale => pointOfSale.Operations)
            .Where(predicate: ps => ps.Id == request.Model.PointOfSaleId
                                    || ps.Id == request.Model.ReceiverId)
            .GroupBy(keySelector: ps => 1)
            .Select(selector: group => new
            {
                Owner = group.First(pointOfSale => pointOfSale.Id == request.Model.PointOfSaleId),
                Receiver = group.First(pointOfSale => pointOfSale.Id == request.Model.ReceiverId),
                GarmentModel = context.Set<GarmentModel>()
                    .Single(garmentModel => garmentModel.Id == request.Model.GarmentModelId)
            })
            .SingleAsync(cancellationToken);

        pair.Owner.Transfer(
            pair.GarmentModel,
            request.Model.Quantity,
            request.Model.Date,
            pair.Receiver);

        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.Result.Ok)
        {
            return Operation.Error(unitOfWork.Result.Exception
                                   ?? new SewingFactoryDatabaseSaveException(
                                       $"{nameof(PointOfSale)} error while saving {nameof(PointOfSale.Transfer)} operation"));
        }

        return Operation.Result();
    }
}
