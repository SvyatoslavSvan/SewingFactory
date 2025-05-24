using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration.Base;
using System.Security.Cryptography.X509Certificates;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration
{
    public class InternalTransferOperationModelConfiguration
        : IEntityTypeConfiguration<InternalTransferOperation>
    {
        public void Configure(EntityTypeBuilder<InternalTransferOperation> builder) => builder.HasOne(it => it.Receiver)
            .WithMany()
            .HasForeignKey(x => x.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
