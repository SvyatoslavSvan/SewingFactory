using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations.Base;
using SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration;

public class OperationModelConfiguration : IdentityModelConfigurationBase<Operation>
{
    protected override void AddBuilder(
        EntityTypeBuilder<Operation> builder)
    {
        builder.OwnsOne(x => x.PriceOnOperationDate);
        builder.Ignore(x => x.DisplayName);
        builder.Ignore(x => x.HistoricalSum);
        builder.Ignore(x => x.CurrentSum);
        
        builder.HasOne(navigationExpression: o => o.Owner)
            .WithMany(navigationExpression: p => p.Operations)
            .HasForeignKey(foreignKeyExpression: x => x.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.StockItem)
            .WithMany(x => x.Operations)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasDiscriminator<string>("OperationType")
            .HasValue<ReceiveOperation>("Receive")
            .HasValue<SaleOperation>("Sale")
            .HasValue<WriteOffOperation>("WriteOff")
            .HasValue<InternalTransferOperation>("InternalTransfer");
    }

    protected override string TableName()
        => "Operations";
}
