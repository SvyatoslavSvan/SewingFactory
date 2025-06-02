using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

public class WorkshopTaskConfiguration : IdentityModelConfigurationBase<WorkshopTask>
{
    protected override string TableName() => "WorkshopTasks";

    protected override void AddBuilder(EntityTypeBuilder<WorkshopTask> builder)
    {
        builder.HasOne(navigationExpression: x => x.Process).WithMany().IsRequired();
        builder.HasOne(navigationExpression: x => x.Document).WithMany(navigationExpression: x => x.Tasks).IsRequired();
        builder.HasMany(navigationExpression: x => x.EmployeeTaskRepeats).WithOne(navigationExpression: x => x.WorkshopTask).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.Ignore(propertyExpression: x => x.EmployeesInvolved);
        builder.Navigation(navigationExpression: x => x.EmployeeTaskRepeats)
            .UsePropertyAccessMode(PropertyAccessMode.Field).HasField("_employeeTaskRepeats");
    }
}