using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.Mapping.Converters;

public sealed class GarmentCategoryStubConverter(ApplicationDbContext dbContext) : ITypeConverter<Guid, GarmentCategory>
{
    public GarmentCategory Convert(Guid source, GarmentCategory destination, ResolutionContext context)
    {
        var tracked = dbContext.ChangeTracker.Entries<GarmentCategory>()
            .FirstOrDefault(predicate: e => (Guid)e.Property("Id").CurrentValue! == source)?
            .Entity;

        if (tracked is not null)
        {
            return tracked;
        }

        var categoryStub = new GarmentCategory("stub", []);
        dbContext.Entry(categoryStub).Property("Id").CurrentValue = source;
        dbContext.Entry(categoryStub).State = EntityState.Unchanged;

        return categoryStub;
    }
}
