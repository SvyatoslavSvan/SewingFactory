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
        var db = unitOfWork.DbContext;
        var pair = await db.Set<PointOfSale>()
            .Include(navigationPropertyPath: ps => ps.StockItems)
            .ThenInclude(navigationPropertyPath: si => si.GarmentModel)
            .Include(navigationPropertyPath: ps => ps.Operations)
            .Where(predicate: ps => ps.Id == request.Model.PointOfSaleId
                                    || ps.Id == request.Model.ReceiverId)
            .GroupBy(keySelector: ps => 1)
            .Select(selector: g => new
            {
                Owner = g.First(ps => ps.Id == request.Model.PointOfSaleId),
                Receiver = g.First(ps => ps.Id == request.Model.ReceiverId),
                GarmentModel = db.Set<GarmentModel>()
                    .Single(gm => gm.Id == request.Model.GarmentModelId)
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
