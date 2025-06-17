using MassTransit;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Common.Messaging.Contracts.GarmentModel;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Publisher;

public sealed class GarmentModelPublisher(IPublishEndpoint publishEndpoint) : IGarmentModelPublisher
{
    public Task PublishCreatedAsync(GarmentModel garmentModel)
        => publishEndpoint.Publish(new GarmentModelCreated(garmentModel.Id, garmentModel.Name, garmentModel.Price.Amount, garmentModel.Category.Id));

    public Task PublishUpdatedAsync(GarmentModel garmentModel)
        => publishEndpoint.Publish(new GarmentModelUpdated(garmentModel.Id, garmentModel.Name, garmentModel.Price.Amount, garmentModel.Category.Id));
}