using MassTransit;
using Moq;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Publisher;
using SewingFactory.Common.Domain.ValueObjects;
using SewingFactory.Common.Messaging.Contracts.GarmentModel;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.GarmentModels.Publisher;

public sealed class GarmentModelPublisherTests
{
    [Fact]
    public async Task PublishCreatedAsync_Sends_Correct_Message()
    {
        var publishEndpoint = new Mock<IPublishEndpoint>();
        var sut = new GarmentModelPublisher(publishEndpoint.Object);
        var category = new GarmentCategory("Category", []);
        var model = new GarmentModel("Model-A","test",[], category, new Money(99));

        await sut.PublishCreatedAsync(model);

        publishEndpoint.Verify(x => x.Publish(
                It.Is<GarmentModelCreated>(m =>
                    m.Id == model.Id &&
                    m.Name == model.Name &&
                    m.Price == model.Price.Amount &&
                    m.GarmentCategoryId == category.Id),
                CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task PublishUpdatedAsync_Sends_Correct_Message()
    {
        var publishEndpoint = new Mock<IPublishEndpoint>();
        var sut = new GarmentModelPublisher(publishEndpoint.Object);
        var category = new GarmentCategory("Category", []);
        var model = new GarmentModel("Model-B", "test", [], category, new Money(99));

        await sut.PublishUpdatedAsync(model);

        publishEndpoint.Verify(x => x.Publish(
                It.Is<GarmentModelUpdated>(m =>
                    m.Id == model.Id &&
                    m.Name == model.Name &&
                    m.Price == model.Price.Amount &&
                    m.GarmentCategoryId == category.Id),
                CancellationToken.None),
            Times.Once);
    }
}