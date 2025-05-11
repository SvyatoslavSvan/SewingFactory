using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration
{
    internal class DepartmentConfiguration : IdentityModelConfigurationBase<Department>
    {
        protected override void AddBuilder(EntityTypeBuilder<Department> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.HasMany(d => d.Employees).WithOne(e => e.Department);
            builder.HasMany(d => d.Documents).WithOne(w => w.Department);
            builder.HasMany(d => d.Processes).WithOne(p => p.Department);
        }

        protected override string TableName() => "Departments";
    }
}
