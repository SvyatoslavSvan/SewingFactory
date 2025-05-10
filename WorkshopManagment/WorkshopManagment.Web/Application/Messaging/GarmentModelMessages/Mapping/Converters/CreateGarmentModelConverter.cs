using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.Mapping.Converters;

public sealed class CreateGarmentModelConverter(IUnitOfWork<ApplicationDbContext> unitOfWork) : ITypeConverter<CreateGarmentModelViewModel, GarmentModel>
{
    public GarmentModel Convert(CreateGarmentModelViewModel source, GarmentModel destination, ResolutionContext context) => new(
        source.Name,
        source.Description,
        StubHelper.GetProcessStubs(source, unitOfWork),
        StubHelper.GetGarmentCategoryStub(source, unitOfWork),
        source.Image
    );
}