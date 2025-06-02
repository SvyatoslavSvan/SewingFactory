using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Base;
using SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration;

public class OperationModelConfiguration : IdentityModelConfigurationBase<Operation>
{
    protected override void AddBuilder(EntityTypeBuilder<Operation> builder)
    {
        builder.HasOne(navigationExpression: o => o.Owner)
            .WithMany(navigationExpression: p => p.Operations).HasForeignKey(foreignKeyExpression: x => x.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasDiscriminator<string>("OperationType")
            .HasValue<ReceiveOperation>("Receive")
            .HasValue<SaleOperation>("Sale")
            .HasValue<WriteOffOperation>("WriteOff")
            .HasValue<InternalTransferOperation>("InternalTransfer");
    }

    protected override string TableName() => "Operations";
}
