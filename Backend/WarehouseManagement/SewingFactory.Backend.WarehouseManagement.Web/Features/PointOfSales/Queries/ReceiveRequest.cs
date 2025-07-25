﻿using Calabonga.OperationResults;
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

public sealed record ReceiveRequest(
    CreateOperationViewModel Model,
    ClaimsPrincipal User
) : IRequest<OperationEmpty<SewingFactoryNotFoundException, Exception>>;

public sealed class ReceiveRequestHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork
) : IRequestHandler<ReceiveRequest, OperationEmpty<SewingFactoryNotFoundException, Exception>>
{
    public async Task<OperationEmpty<SewingFactoryNotFoundException, Exception>> Handle(
        ReceiveRequest request,
        CancellationToken cancellationToken)
    {
        var db = unitOfWork.DbContext;

        var pair = await db.Set<PointOfSale>()
            .Include(navigationPropertyPath: pointOfSale => pointOfSale.StockItems)
            .ThenInclude(navigationPropertyPath: stockItem => stockItem.GarmentModel)
            .Include(navigationPropertyPath: pointOfSale => pointOfSale.Operations)
            .Where(predicate: pointOfSale => pointOfSale.Id == request.Model.PointOfSaleId)
            .GroupBy(keySelector: pointOfSale => 1)
            .Select(selector: grouping => new
            {
                PointOfSale = grouping.First(),
                GarmentModel = db.Set<GarmentModel>()
                    .Single(garmentModel => garmentModel.Id == request.Model.GarmentModelId)
            })
            .SingleAsync(cancellationToken);

        pair.PointOfSale.Receive(pair.GarmentModel, request.Model.Quantity, request.Model.Date);

        await unitOfWork.SaveChangesAsync();
        if (!unitOfWork.Result.Ok)
        {
            return Operation.Error(unitOfWork.Result.Exception
                                   ?? new SewingFactoryDatabaseSaveException(
                                       $"{nameof(PointOfSale)} error while saving {nameof(PointOfSale.Receive)} operation"));
        }

        return Operation.Result();
    }
}
