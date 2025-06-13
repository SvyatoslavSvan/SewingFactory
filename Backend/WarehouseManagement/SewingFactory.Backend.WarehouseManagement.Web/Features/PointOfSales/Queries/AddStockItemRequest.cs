using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.StockItems;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;
using Operation = Calabonga.OperationResults.Operation;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Queries;

public sealed record AddStockItemRequest(
    AddStockItemViewModel Model,
    ClaimsPrincipal User) : IRequest<Operation<StockItemReadViewModel, SewingFactoryNotFoundException, Exception>>;

public sealed class AddStockItemRequestHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper) : IRequestHandler<AddStockItemRequest, Operation<StockItemReadViewModel, SewingFactoryNotFoundException, Exception>>
{
    public async Task<Operation<StockItemReadViewModel, SewingFactoryNotFoundException, Exception>> Handle(
        AddStockItemRequest request,
        CancellationToken cancellationToken)
    {
        var dbContext = unitOfWork.DbContext;

        var pair = await (
                from pointOfSale in dbContext.Set<PointOfSale>()
                    .AsTracking()
                    .Where(predicate: pointOfSale => pointOfSale.Id == request.Model.PointOfSaleId)
                    .Include(navigationPropertyPath: pointOfSale => pointOfSale.StockItems)
                    .ThenInclude(navigationPropertyPath: stockItem => stockItem.GarmentModel)
                from garmentModel in dbContext.Set<GarmentModel>()
                    .AsTracking()
                    .Where(garmentModel => garmentModel.Id == request.Model.GarmentModelId)
                select new { PointOfSale = pointOfSale, GarmentModel = garmentModel })
            .FirstOrDefaultAsync(cancellationToken);

        if (pair is null)
        {
            return Operation.Error(
                new SewingFactoryNotFoundException($"{nameof(PointOfSale)} or {nameof(GarmentModel)} with {request.Model.PointOfSaleId} {request.Model.GarmentModelId} was not found"));
        }

        var stockItem = pair.PointOfSale.AddStockItem(pair.GarmentModel);

        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.Result.Ok)
        {
            return Operation.Error(unitOfWork.Result.Exception ?? new SewingFactoryDatabaseSaveException($"{nameof(PointOfSale)} error while saving {nameof(PointOfSale.AddStockItem)} operation"));
        }

        return Operation.Result(mapper.Map<StockItemReadViewModel>(stockItem));
    }
}
