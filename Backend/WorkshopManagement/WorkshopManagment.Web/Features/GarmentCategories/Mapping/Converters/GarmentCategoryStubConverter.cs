using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Mapping.Converters;

public sealed class GarmentCategoryStubConverter(ApplicationDbContext dbContext) : ITypeConverter<Guid, GarmentCategory>
{
    public GarmentCategory Convert(Guid source, GarmentCategory destination, ResolutionContext context)
    {
        var stub = new GarmentCategory("stub", []);
        dbContext.Entry(stub).Property("Id").CurrentValue = source;
        dbContext.Entry(stub).State = EntityState.Unchanged;

        return stub;
    }
}