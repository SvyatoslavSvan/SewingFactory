using Calabonga.UnitOfWork;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Tests.Common;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Consumers;
using SewingFactory.Common.Domain.ValueObjects;
using SewingFactory.Common.Messaging.Contracts.GarmentModel;

namespace SewingFactory.Backend.WarehouseManagement.Tests.Features.GarmentModels.Consumers;

public sealed class GarmentModelUpdatedConsumerTests
{
    [Fact]
    public async Task Consume_Updates_Model()
    {
        var provider      = TestHelpers.BuildServices();
        var uow           = provider.GetRequiredService<IUnitOfWork>();
        var logger        = new Mock<ILogger<GarmentModelUpdatedConsumer>>();
        var consumer      = new GarmentModelUpdatedConsumer(uow, logger.Object);

        var oldCatId      = Guid.NewGuid();
        var newCatId      = Guid.NewGuid();
        var oldCategory   = new GarmentCategory("OldCategory", [], oldCatId);
        var newCategory   = new GarmentCategory("NewCategory", [], newCatId);
        await uow.GetRepository<GarmentCategory>().InsertAsync(oldCategory, newCategory);

        var modelId       = Guid.NewGuid();
        await uow.GetRepository<GarmentModel>().InsertAsync(new GarmentModel("OldModel", oldCategory, new Money(10m), modelId));
        await uow.SaveChangesAsync();

        var ctx = new Mock<ConsumeContext<GarmentModelUpdated>>();
        ctx.SetupGet(c => c.Message).Returns(new GarmentModelUpdated(modelId, "UpdatedModel", 120m, newCatId));
        ctx.SetupGet(c => c.CancellationToken).Returns(CancellationToken.None);

        await consumer.Consume(ctx.Object);

        var model = await uow.GetRepository<GarmentModel>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == modelId, include: q => q.Include(m => m.Category));
        Assert.NotNull(model);
        Assert.Equal("UpdatedModel", model!.Name);
        Assert.Equal(120m, model.Price.Amount);
        Assert.Equal(newCatId, model.Category.Id);
    }
}
