using AutoMapper;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentCategoryMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentCategoryMessages;

public class GarmentCategoryMappingProfile : Profile
{
    public GarmentCategoryMappingProfile()
    {
        CreateMap<CreateGarmentCategoryViewModel, GarmentCategory>()
            .ConstructUsing(ctor: src =>
                new GarmentCategory(src.Name, new List<GarmentModel>()))
            .ForMember(destinationMember: dest => dest.GarmentModels, memberOptions: opt => opt.Ignore());

        CreateMap<UpdateGarmentCategoryViewModel, GarmentCategory>()
            .ForMember(destinationMember: d => d.Name, memberOptions: o => o.MapFrom(mapExpression: s => s.Name))
            .ForAllMembers(memberOptions: o => o.Ignore());

        CreateMap<GarmentCategory, ReadGarmentCategoryViewModel>();
    }
}