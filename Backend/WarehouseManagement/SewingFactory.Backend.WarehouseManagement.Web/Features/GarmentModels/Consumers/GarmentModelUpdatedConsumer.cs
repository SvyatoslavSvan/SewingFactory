using Calabonga.UnitOfWork;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;
using SewingFactory.Common.Messaging.Contracts.GarmentModel;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Consumers;

public sealed class GarmentModelUpdatedConsumer(IUnitOfWork unitOfWork, ILogger<GarmentModelUpdatedConsumer> logger) : IConsumer<GarmentModelUpdated>
{
    public async Task Consume(ConsumeContext<GarmentModelUpdated> context)
    {
        var garmentModel = await unitOfWork.GetRepository<GarmentModel>()
            .GetFirstOrDefaultAsync(predicate: model => model.Id == context.Message.Id,
                include: q => q.Include(model => model.Category),
                trackingType: TrackingType.Tracking);
        EnsureGarmentModelFound(context, garmentModel);
        await UpdateProperties(context, garmentModel!);
        EnsureUpdateSucceeded();
    }

    private void EnsureGarmentModelFound(ConsumeContext<GarmentModelUpdated> context, GarmentModel? garmentModel)
    {
        if (garmentModel is null)
        {
            logger.LogError("GarmentModel with Id={Id} was not found", context.Message.Id);
            throw new SewingFactoryNotFoundException($"GarmentModel with Id={context.Message.Id} was not found");
        }
    }

    private void EnsureUpdateSucceeded()
    {
        if (!unitOfWork.Result.Ok)
        {
            logger.LogError("Error while updating GarmentModel in the Consumer");
            throw new SewingFactoryInvalidOperationException($"Error while updating GarmentModel in the Consumer: {unitOfWork.Result.Exception!.Message}");
        }
    }

    private async Task UpdateProperties(ConsumeContext<GarmentModelUpdated> context, GarmentModel garmentModel)
    {
        garmentModel.Name = context.Message.Name;
        garmentModel.Price = new Money(context.Message.Price);
        if (garmentModel.Category.Id != context.Message.GarmentCategoryId)
        {
            garmentModel.Category = await unitOfWork.GetRepository<GarmentCategory>()
                                        .GetFirstOrDefaultAsync(predicate: model => model.Id == context.Message.GarmentCategoryId,
                                            trackingType: TrackingType.Tracking) ??
                                    throw new SewingFactoryNotFoundException($"GarmentCategory with Id={context.Message.GarmentCategoryId} was not found");;
        }
        await unitOfWork.SaveChangesAsync();
    }
}
