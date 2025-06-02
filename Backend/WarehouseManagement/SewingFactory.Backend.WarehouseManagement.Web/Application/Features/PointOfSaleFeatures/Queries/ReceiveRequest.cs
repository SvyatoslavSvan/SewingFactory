using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Operations;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;
using Operation = Calabonga.OperationResults.Operation;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;

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
            .Include(navigationPropertyPath: ps => ps.StockItems)
            .ThenInclude(navigationPropertyPath: si => si.GarmentModel)
            .Include(navigationPropertyPath: ps => ps.Operations)
            .Where(predicate: ps => ps.Id == request.Model.PointOfSaleId)
            .GroupBy(keySelector: ps => 1)
            .Select(selector: g => new
            {
                PointOfSale = g.First(),
                GarmentModel = db.Set<GarmentModel>()
                    .Single(gm => gm.Id == request.Model.GarmentModelId)
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
