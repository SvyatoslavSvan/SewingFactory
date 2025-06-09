using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration;

public class InternalTransferOperationModelConfiguration
    : IEntityTypeConfiguration<InternalTransferOperation>
{
    public void Configure(EntityTypeBuilder<InternalTransferOperation> builder) => builder.HasOne(navigationExpression: it => it.Receiver)
        .WithMany()
        .HasForeignKey(foreignKeyExpression: x => x.ReceiverId)
        .OnDelete(DeleteBehavior.Restrict);
}
