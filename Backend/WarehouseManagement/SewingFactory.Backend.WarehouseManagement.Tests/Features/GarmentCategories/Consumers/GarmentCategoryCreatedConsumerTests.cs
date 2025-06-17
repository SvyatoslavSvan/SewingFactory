using Calabonga.UnitOfWork;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Tests.Common;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Consumers;
using SewingFactory.Common.Messaging.Contracts.GarmentCategories;

namespace SewingFactory.Backend.WarehouseManagement.Tests.Features.GarmentCategories.Consumers;

public sealed class GarmentCategoryCreatedConsumerTests
{
    [Fact]
    public async Task Consume_Inserts_Category()
    {
        var provider   = TestHelpers.BuildServices();
        var uow        = provider.GetRequiredService<IUnitOfWork>();
        var logger     = new Mock<ILogger<GarmentCategoryCreatedConsumer>>();
        var consumer   = new GarmentCategoryCreatedConsumer(uow, logger.Object);

        var categoryId = Guid.NewGuid();
        var ctx        = new Mock<ConsumeContext<GarmentCategoryCreated>>();
        ctx.SetupGet(c => c.Message).Returns(new GarmentCategoryCreated(categoryId, "Category1"));
        ctx.SetupGet(c => c.CancellationToken).Returns(CancellationToken.None);

        await consumer.Consume(ctx.Object);

        var category = await uow.GetRepository<GarmentCategory>().GetFirstOrDefaultAsync(predicate: x => x.Id == categoryId);
        Assert.NotNull(category);
        Assert.Equal("Category1", category!.Name);
    }
}
