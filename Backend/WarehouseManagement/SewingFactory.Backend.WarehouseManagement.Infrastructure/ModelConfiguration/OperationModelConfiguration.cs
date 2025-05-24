using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration
{
    public class OperationModelConfiguration : IdentityModelConfigurationBase<Operation>
    {
        protected override void AddBuilder(EntityTypeBuilder<Operation> builder)
        {
            builder.Property(o => o.OperationType)
                .HasConversion<string>()
                .IsRequired();

            builder.HasDiscriminator(o => o.OperationType)
                .HasValue<Operation>(OperationType.Sale)
                .HasValue<Operation>(OperationType.Receive)
                .HasValue<Operation>(OperationType.WriteOff)
                .HasValue<InternalTransferOperation>(OperationType.InternalTransfer);

            builder.HasOne(o => o.Owner)
                .WithMany(p => p.Operations).HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override string TableName() => "Operations";
    }
}
