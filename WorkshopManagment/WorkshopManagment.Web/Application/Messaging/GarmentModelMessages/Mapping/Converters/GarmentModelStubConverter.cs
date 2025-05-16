using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.Mapping.Converters
{
    public class GarmentModelStubConverter(IUnitOfWork<ApplicationDbContext> unitOfWork) : ITypeConverter<Guid,GarmentModel>
    {
        public GarmentModel Convert(Guid source, GarmentModel destination, ResolutionContext context)
        {
            var stub = new GarmentModel("stub", "stub", [],new GarmentCategory("stub", []));
            unitOfWork.DbContext.Entry(stub).Property("Id").CurrentValue = source;
            unitOfWork.DbContext.Entry(stub).State = EntityState.Unchanged;
            return stub;
        }
    }
}
