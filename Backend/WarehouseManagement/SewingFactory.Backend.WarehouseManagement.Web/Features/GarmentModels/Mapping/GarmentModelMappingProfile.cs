using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Mapping;

public sealed class GarmentModelMappingProfile : AutoMapper.Profile
{
    public GarmentModelMappingProfile()
    {
        CreateMap<GarmentModel, GarmentModelReadViewModel>()
            .ForMember(destinationMember: x => x.Price,
                memberOptions: opt => opt.MapFrom(mapExpression: src => src.Price.Amount));

        CreateMap<GarmentModelCreateViewModel, GarmentModel>()
            .ConstructUsing(ctor: (
                    src,
                    ctx) =>
                new GarmentModel(
                    src.Name,
                    ctx.Mapper.Map<GarmentCategory>(src.CategoryId),
                    Money.Zero
                ))
            .ForMember(destinationMember: x => x.Category,
                memberOptions: o => o.Ignore());

        CreateMap<GarmentModelEditViewModel, GarmentModel>()
            .ForMember(destinationMember: d => d.Id,
                memberOptions: opt => opt.Ignore())
            .ForMember(destinationMember: d => d.Price,
                memberOptions: opt => opt.MapFrom(mapExpression: src => new Money(src.Price)))
            .ForMember(destinationMember: d => d.Category,
                memberOptions: opt => opt.MapFrom(mapExpression: s => s.CategoryId))
            .ForMember(destinationMember: x => x.Name,
                memberOptions: o => o.MapFrom(mapExpression: x => x.Name));
    }
}
