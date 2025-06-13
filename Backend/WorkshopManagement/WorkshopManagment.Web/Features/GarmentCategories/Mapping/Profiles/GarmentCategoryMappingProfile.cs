using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Extensions;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Mapping.Converters;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Mapping.Profiles;

public sealed class GarmentCategoryMappingProfile : Profile
{
    public GarmentCategoryMappingProfile()
    {
        CreateMap<Guid, GarmentCategory>().ConvertUsing<GarmentCategoryStubConverter>();
        
        CreateMap<CreateGarmentCategoryViewModel, GarmentCategory>()
            .ConstructUsing(ctor: src =>
                new GarmentCategory(src.Name, new List<GarmentModel>()))
            .ForMember(destinationMember: dest => dest.GarmentModels, memberOptions: opt => opt.Ignore());

        CreateMap<UpdateGarmentCategoryViewModel, GarmentCategory>()
            .ForMember(destinationMember: d => d.Name, memberOptions: o => o.MapFrom(mapExpression: s => s.Name))
            .ForAllOtherMembers(memberOptions: o => o.Ignore());

        CreateMap<GarmentCategory, ReadGarmentCategoryViewModel>();
    }
}