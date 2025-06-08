using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.StockItems;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;
using Operation = Calabonga.OperationResults.Operation;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;

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
                from ps in dbContext.Set<PointOfSale>()
                    .AsTracking()
                    .Where(predicate: ps => ps.Id == request.Model.PointOfSaleId)
                    .Include(navigationPropertyPath: x => x.StockItems)
                    .ThenInclude(navigationPropertyPath: x => x.GarmentModel)
                from gm in dbContext.Set<GarmentModel>()
                    .AsTracking()
                    .Where(gm => gm.Id == request.Model.GarmentModelId)
                select new { PointOfSale = ps, GarmentModel = gm })
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
