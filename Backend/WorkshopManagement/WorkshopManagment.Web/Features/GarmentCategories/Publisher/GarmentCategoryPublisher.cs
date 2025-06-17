using MassTransit;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Common.Messaging.Contracts.GarmentCategories;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Publisher;

public sealed class GarmentCategoryPublisher(IPublishEndpoint publishEndpoint) : IGarmentCategoryPublisher
{
    public Task PublishCreatedAsync(GarmentCategory category) => publishEndpoint.Publish(new GarmentCategoryCreated(category.Id, category.Name));

    public Task PublishUpdatedAsync(GarmentCategory category) => publishEndpoint.Publish(new GarmentCategoryUpdated(category.Id, category.Name));
}