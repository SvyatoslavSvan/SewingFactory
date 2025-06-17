using Calabonga.UnitOfWork;
using MassTransit;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Messaging.Contracts.GarmentCategories;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Consumers;

public sealed class GarmentCategoryUpdatedConsumer(IUnitOfWork unitOfWork, ILogger<GarmentCategoryUpdatedConsumer> logger) : IConsumer<GarmentCategoryUpdated>
{
    public async Task Consume(ConsumeContext<GarmentCategoryUpdated> context)
    {
        var garmentCategory = await unitOfWork.GetRepository<GarmentCategory>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == context.Message.Id,
                trackingType: TrackingType.Tracking);;
        EnsureGarmentCategoryFound(context, garmentCategory);
        await UpdateProperties(context, garmentCategory);
        EnsureUpdateSucceeded();
    }

    private async Task UpdateProperties(ConsumeContext<GarmentCategoryUpdated> context, GarmentCategory? garmentCategory)
    {
        garmentCategory!.Name = context.Message.Name;
        await unitOfWork.SaveChangesAsync();
    }

    private static void EnsureGarmentCategoryFound(ConsumeContext<GarmentCategoryUpdated> context, GarmentCategory? garmentCategory)
    {
        if (garmentCategory is null)
        {
            throw new SewingFactoryNotFoundException($"GarmentCategory with Id={context.Message.Id} was not found");
        }
    }

    private void EnsureUpdateSucceeded()
    {
        if (!unitOfWork.Result.Ok)
        {
            logger.LogError("Error while updating GarmentCategory in the Consumer");

            throw new SewingFactoryInvalidOperationException($"Error while updating GarmentCategory in the Consumer: {unitOfWork.Result.Exception!.Message}");
        }
    }
}
