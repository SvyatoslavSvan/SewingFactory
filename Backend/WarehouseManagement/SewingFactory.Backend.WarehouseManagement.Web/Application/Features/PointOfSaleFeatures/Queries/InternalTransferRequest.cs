using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using SewingFactory.Common.Domain.Exceptions;
using System.Security.Claims;
using Operation = Calabonga.OperationResults.Operation;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries
{
    public record InternalTransferRequest(
    InternalTransferViewModel Model,
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
                .Include(ps => ps.StockItems)
                .ThenInclude(si => si.GarmentModel)
                .Include(ps => ps.Operations)
                .Where(ps => ps.Id == request.Model.PointOfSaleId
                             || ps.Id == request.Model.ReceiverId)
                .GroupBy(ps => 1)
                .Select(g => new
                {
                    Owner = g.First(ps => ps.Id == request.Model.PointOfSaleId),
                    Receiver = g.First(ps => ps.Id == request.Model.ReceiverId),
                    GarmentModel = db.Set<GarmentModel>()
                        .Single(gm => gm.Id == request.Model.GarmentModelId)
                })
                .SingleAsync(cancellationToken);

            pair.Owner.Transfer(
                model: pair.GarmentModel,
                quantityToTransfer: request.Model.Quantity,
                date: request.Model.Date,
                receiver: pair.Receiver);

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

}
