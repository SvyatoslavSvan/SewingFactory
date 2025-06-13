using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Mapping.Converters;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class GarmentModelStubConverter(IUnitOfWork<ApplicationDbContext> unitOfWork) : ITypeConverter<Guid, GarmentModel>
{
    public GarmentModel Convert(Guid source, GarmentModel destination, ResolutionContext context)
    {
        var stub = new GarmentModel("stub", "stub", [], new GarmentCategory("stub", []), Money.Zero);
        unitOfWork.DbContext.Entry(stub).Property("Id").CurrentValue = source;
        unitOfWork.DbContext.Entry(stub).State = EntityState.Unchanged;

        return stub;
    }
}