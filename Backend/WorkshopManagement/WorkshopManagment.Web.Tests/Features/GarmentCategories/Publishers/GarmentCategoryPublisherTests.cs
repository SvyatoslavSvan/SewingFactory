using MassTransit;
using Moq;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Publisher;
using SewingFactory.Common.Messaging.Contracts.GarmentCategories;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.GarmentCategories.Publishers;

public sealed class GarmentCategoryPublisherTests
{
    [Fact]
    public async Task PublishCreatedAsync_Sends_Correct_Message()
    {
        // Arrange
        var publishEndpoint = new Mock<IPublishEndpoint>();
        var sut = new GarmentCategoryPublisher(publishEndpoint.Object);
        var category = new GarmentCategory("Shirts", []);

        // Act
        await sut.PublishCreatedAsync(category);

        // Assert
        publishEndpoint.Verify(x => x.Publish(
                It.Is<GarmentCategoryCreated>(m =>
                    m.Id == category.Id &&
                    m.Name == category.Name),
                CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task PublishUpdatedAsync_Sends_Correct_Message()
    {
        var publishEndpoint = new Mock<IPublishEndpoint>();
        var sut = new GarmentCategoryPublisher(publishEndpoint.Object);
        var category = new GarmentCategory("T-Shirts", []);

        await sut.PublishUpdatedAsync(category);

        publishEndpoint.Verify(x => x.Publish(
                It.Is<GarmentCategoryUpdated>(m =>
                    m.Id == category.Id &&
                    m.Name == category.Name),
                CancellationToken.None),
            Times.Once);
    }
}