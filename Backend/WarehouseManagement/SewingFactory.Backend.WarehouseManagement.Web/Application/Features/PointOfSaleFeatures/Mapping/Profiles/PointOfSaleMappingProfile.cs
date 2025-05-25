using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Extensions;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Mapping.Profiles
{
    public class PointOfSaleMappingProfile : AutoMapper.Profile
    {
        public PointOfSaleMappingProfile()
        {
            CreateMap<PointOfSale, PointOfSaleReadViewModel>()
                .ForMember(x => x.Name, o => o.MapFrom(x => x.Name))
                .ForMember(x => x.Id, o => o.MapFrom(x => x.Id))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PointOfSaleCreateViewModel, PointOfSale>()
                .ConstructUsing(src => new PointOfSale(
                    src.Name
                ))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PointOfSaleEditViewModel, PointOfSale>()
                .ForMember(x => x.Name, o => o.MapFrom(x => x.Name))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
