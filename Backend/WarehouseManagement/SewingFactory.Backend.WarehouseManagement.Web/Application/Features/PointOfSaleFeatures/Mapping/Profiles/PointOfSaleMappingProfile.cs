using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.StockItems;
using SewingFactory.Backend.WorkshopManagement.Web.Extensions;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Mapping.Profiles
{
    public class PointOfSaleMappingProfile : AutoMapper.Profile
    {
        public PointOfSaleMappingProfile()
        {
            CreateMap<PointOfSale, PointOfSaleReadViewModel>()
                .ForMember(x => x.Name,
                    o => o.MapFrom(x => x.Name))
                .ForMember(x => x.Id,
                    o => o.MapFrom(x => x.Id))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PointOfSaleCreateViewModel, PointOfSale>()
                .ConstructUsing(src => new PointOfSale(
                    src.Name
                ))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PointOfSaleEditViewModel, PointOfSale>()
                .ForMember(x => x.Name,
                    o => o.MapFrom(x => x.Name))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PointOfSale, PointOfSaleDetailsReadViewModel>()
                .ForMember(x => x.Id,
                    o => o.MapFrom(x => x.Id))
                .ForMember(x => x.Name,
                    o => o.MapFrom(x => x.Name))
                .ForMember(x => x.StockItems,
                o => o.MapFrom(x => x.StockItems))
                .ForMember(x => x.Operations,
                    o => o.MapFrom(x => x.Operations))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<StockItem, StockItemReadViewModel>()
                .ForMember(x => x.Quantity,
                    o => o.MapFrom(x => x.Quantity))
                .ForMember(x => x.GarmentModel,
                    o => o.MapFrom(x => x.GarmentModel));

        }
    }
}
