using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.Mapping;

public sealed class GarmentModelMappingProfile : AutoMapper.Profile
{
    public GarmentModelMappingProfile()
    {
        CreateMap<GarmentModel, GarmentModelReadViewModel>()
            .ForMember(x => x.Price,
                opt => opt.MapFrom(mapExpression: src => src.Price.Amount));

        CreateMap<GarmentModelCreateViewModel, GarmentModel>()
            .ConstructUsing((
                    src,
                    ctx) =>
                new GarmentModel(
                    src.Name,
                    ctx.Mapper.Map<GarmentCategory>(src.CategoryId),
                    Money.Zero
                ))
            .ForMember(x => x.Category,
                o => o.Ignore());

        CreateMap<GarmentModelEditViewModel, GarmentModel>()
            .ForMember(d => d.Id,
                opt => opt.Ignore())
            .ForMember(d => d.Price,
                opt => opt.MapFrom(mapExpression: src => new Money(src.Price)))
            .ForMember(d => d.Category,
                opt => opt.MapFrom(s => s.CategoryId))
            .ForMember(x => x.Name,
                o => o.MapFrom(x => x.Name));
    }
}
