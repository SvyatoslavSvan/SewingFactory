using Calabonga.UnitOfWork;
using MassTransit;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Common.Domain.ValueObjects;
using SewingFactory.Common.Messaging.Contracts.GarmentModel;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Consumers;

public sealed class GarmentModelCreatedConsumer(IUnitOfWork unitOfWork, ILogger<GarmentModelCreatedConsumer> logger) : IConsumer<GarmentModelCreated>
{
    public async Task Consume(ConsumeContext<GarmentModelCreated> context)
    {
        var category = await unitOfWork.GetRepository<GarmentCategory>().GetFirstOrDefaultAsync(
            predicate: garmentCategory => garmentCategory.Id == context.Message.GarmentCategoryId, trackingType: TrackingType.Tracking);

        EnsureCategoryFound(context, category);
        await CreateGarmentModel(context, category);
        EnsureCreatedSucceeded();
    }

    private void EnsureCreatedSucceeded()
    {
        if (!unitOfWork.Result.Ok)
        {
            logger.LogError("Error while saving GarmentModel in the Consumer");

            throw new InvalidOperationException($"Failed to create GarmentModel in the Consumer: {unitOfWork.Result.Exception!.Message}");
        }
    }

    private async Task CreateGarmentModel(ConsumeContext<GarmentModelCreated> context, GarmentCategory? category)
    {
        var garmentModel = new GarmentModel(context.Message.Name, category, new Money(context.Message.Price), context.Message.Id);
        await unitOfWork.GetRepository<GarmentModel>().InsertAsync(garmentModel);
        await unitOfWork.SaveChangesAsync();
    }

    private void EnsureCategoryFound(ConsumeContext<GarmentModelCreated> context, GarmentCategory? category)
    {
        if (category is null)
        {
            logger.LogError($"GarmentCategory with Id={context.Message.GarmentCategoryId} was not found", context.Message.GarmentCategoryId);
            throw new InvalidOperationException($"GarmentCategory with Id={context.Message.GarmentCategoryId} was not found");
        }
    }
}
