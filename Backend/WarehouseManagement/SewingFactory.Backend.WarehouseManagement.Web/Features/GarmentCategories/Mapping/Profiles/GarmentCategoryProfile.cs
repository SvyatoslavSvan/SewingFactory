using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Mapping.Converters;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Mapping.Profiles;

public sealed class GarmentCategoryProfile : AutoMapper.Profile
{
    public GarmentCategoryProfile()
    {
        CreateMap<Guid, GarmentCategory>()
            .ConvertUsing<GarmentCategoryStubConverter>();

        CreateMap<GarmentCategory, GarmentCategoryReadViewModel>();

        CreateMap<GarmentCategoryCreateViewModel, GarmentCategory>()
            .ConstructUsing(ctor: src => new GarmentCategory(
                src.Name,
                new List<GarmentModel>()
            )).ForMember(destinationMember: x => x.Products, memberOptions: opt => opt.Ignore());

        CreateMap<GarmentCategoryEditViewModel, GarmentCategory>()
            .ForMember(destinationMember: dest => dest.Products,
                memberOptions: opt => opt.Ignore())
            .ForMember(destinationMember: dest => dest.Id,
                memberOptions: opt => opt.Ignore())
            .ForMember(destinationMember: x => x.Products,
                memberOptions: opt => opt.Ignore())
            ;
    }
}
