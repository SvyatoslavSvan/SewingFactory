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

public sealed class GarmentCategoryUpdatedConsumerTests
{
    [Fact]
    public async Task Consume_Updates_Category()
    {
        var provider   = TestHelpers.BuildServices();
        var uow        = provider.GetRequiredService<IUnitOfWork>();
        var logger     = new Mock<ILogger<GarmentCategoryUpdatedConsumer>>();
        var consumer   = new GarmentCategoryUpdatedConsumer(uow, logger.Object);

        var categoryId = Guid.NewGuid();
        await uow.GetRepository<GarmentCategory>().InsertAsync(new GarmentCategory("OldName", [], categoryId));
        await uow.SaveChangesAsync();

        var ctx = new Mock<ConsumeContext<GarmentCategoryUpdated>>();
        ctx.SetupGet(c => c.Message).Returns(new GarmentCategoryUpdated(categoryId, "NewName"));
        ctx.SetupGet(c => c.CancellationToken).Returns(CancellationToken.None);

        await consumer.Consume(ctx.Object);

        var category = await uow.GetRepository<GarmentCategory>().GetFirstOrDefaultAsync(predicate: x => x.Id == categoryId);
        Assert.NotNull(category);
        Assert.Equal("NewName", category!.Name);
    }
}
