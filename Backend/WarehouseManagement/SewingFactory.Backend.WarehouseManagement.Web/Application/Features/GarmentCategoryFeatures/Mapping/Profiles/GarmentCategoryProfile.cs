using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.Mapping.Converters;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.Mapping.Profiles;

public sealed class GarmentCategoryProfile : AutoMapper.Profile
{
    public GarmentCategoryProfile()
    {
        CreateMap<Guid, GarmentCategory>()
            .ConvertUsing<GarmentCategoryStubConverter>();

        CreateMap<GarmentCategory, GarmentCategoryReadViewModel>();

        CreateMap<GarmentCategoryCreateViewModel, GarmentCategory>()
            .ConstructUsing(src => new GarmentCategory(
                src.Name,
                new List<GarmentModel>()
            )).ForMember(x => x.Products, opt => opt.Ignore());

        CreateMap<GarmentCategoryEditViewModel, GarmentCategory>()
            .ForMember(dest => dest.Products,
                opt => opt.Ignore())
            .ForMember(dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(x => x.Products,
                opt => opt.Ignore())
            ;
    }
}
