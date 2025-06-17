using Calabonga.UnitOfWork;
using MassTransit;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Messaging.Contracts.GarmentCategories;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Consumers;

public sealed class GarmentCategoryCreatedConsumer(IUnitOfWork unitOfWork, ILogger<GarmentCategoryCreatedConsumer> logger) : IConsumer<GarmentCategoryCreated>
{
    public async Task Consume(ConsumeContext<GarmentCategoryCreated> context)
    {
        await CreateGarmentCategory(context);
        EnsureCreatedSucceeded();
    }

    private void EnsureCreatedSucceeded()
    {
        if (!unitOfWork.Result.Ok)
        {
            logger.LogError("Error while saving GarmentCategory in the Consumer");

            throw new SewingFactoryInvalidOperationException($"Failed to create GarmentCategory in the Consumer: {unitOfWork.Result.Exception!.Message}");
        }
    }

    private async Task CreateGarmentCategory(ConsumeContext<GarmentCategoryCreated> context)
    {
        var garmentCategory = new GarmentCategory(context.Message.Name, [], context.Message.Id);
        await unitOfWork.GetRepository<GarmentCategory>().InsertAsync(garmentCategory);
        await unitOfWork.SaveChangesAsync();
    }
}
