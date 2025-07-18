﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

public class EmployeeTaskRepeatConfiguration : IdentityModelConfigurationBase<EmployeeTaskRepeat>
{
    protected override string TableName() => "EmployeeTaskRepeats";

    protected override void AddBuilder(EntityTypeBuilder<EmployeeTaskRepeat> builder)
    {
        builder.Property(propertyExpression: x => x.Repeats).IsRequired();
        builder
            .HasOne(navigationExpression: r => r.WorkShopEmployee)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(navigationExpression: r => r.WorkshopTask)
            .WithMany(navigationExpression: t => t.EmployeeTaskRepeats)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}