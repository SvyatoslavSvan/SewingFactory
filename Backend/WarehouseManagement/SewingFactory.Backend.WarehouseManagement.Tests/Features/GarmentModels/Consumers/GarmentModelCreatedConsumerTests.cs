using Calabonga.UnitOfWork;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Tests.Common;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Consumers;
using SewingFactory.Common.Messaging.Contracts.GarmentModel;

namespace SewingFactory.Backend.WarehouseManagement.Tests.Features.GarmentModels.Consumers;

public sealed class GarmentModelCreatedConsumerTests
{
    [Fact]
    public async Task Consume_Inserts_Model()
    {
        var provider   = TestHelpers.BuildServices();
        var uow        = provider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var logger     = new Mock<ILogger<GarmentModelCreatedConsumer>>();
        var consumer   = new GarmentModelCreatedConsumer(uow, logger.Object);

        var categoryId = Guid.NewGuid();
        await uow.GetRepository<GarmentCategory>().InsertAsync(new GarmentCategory("Category", [], categoryId));
        await uow.SaveChangesAsync();
        uow.DbContext.ChangeTracker.Clear();
        var modelId = Guid.NewGuid();
        var ctx     = new Mock<ConsumeContext<GarmentModelCreated>>();
        ctx.SetupGet(c => c.Message).Returns(new GarmentModelCreated(modelId, "Model1", 50m, categoryId));
        ctx.SetupGet(c => c.CancellationToken).Returns(CancellationToken.None);

        await consumer.Consume(ctx.Object);

        var model = await uow.GetRepository<GarmentModel>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == modelId,
                include: q => q.Include(m => m.Category), 
                trackingType: TrackingType.NoTracking);
        Assert.NotNull(model);
        Assert.Equal("Model1", model!.Name);
        Assert.Equal(50m, model.Price.Amount);
        Assert.Equal(categoryId, model.Category.Id);
    }
}
