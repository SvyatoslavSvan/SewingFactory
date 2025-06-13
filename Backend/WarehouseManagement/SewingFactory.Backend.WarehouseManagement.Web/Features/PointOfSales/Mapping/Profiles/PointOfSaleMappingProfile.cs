using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Extensions;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.StockItems;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Mapping.Profiles;

public class PointOfSaleMappingProfile : AutoMapper.Profile
{
    public PointOfSaleMappingProfile()
    {
        CreateMap<PointOfSale, PointOfSaleReadViewModel>()
            .ForMember(destinationMember: x => x.Name,
                memberOptions: o => o.MapFrom(mapExpression: x => x.Name))
            .ForMember(destinationMember: x => x.Id,
                memberOptions: o => o.MapFrom(mapExpression: x => x.Id))
            .ForAllOtherMembers(memberOptions: opt => opt.Ignore());

        CreateMap<PointOfSaleCreateViewModel, PointOfSale>()
            .ConstructUsing(ctor: src => new PointOfSale(
                src.Name
            ))
            .ForAllOtherMembers(memberOptions: opt => opt.Ignore());

        CreateMap<PointOfSaleEditViewModel, PointOfSale>()
            .ForMember(destinationMember: x => x.Name,
                memberOptions: o => o.MapFrom(mapExpression: x => x.Name))
            .ForAllOtherMembers(memberOptions: opt => opt.Ignore());

        CreateMap<PointOfSale, PointOfSaleDetailsReadViewModel>()
            .ForMember(destinationMember: x => x.Id,
                memberOptions: o => o.MapFrom(mapExpression: x => x.Id))
            .ForMember(destinationMember: x => x.Name,
                memberOptions: o => o.MapFrom(mapExpression: x => x.Name))
            .ForMember(destinationMember: x => x.StockItems,
                memberOptions: o => o.MapFrom(mapExpression: x => x.StockItems))
            .ForMember(destinationMember: x => x.Operations,
                memberOptions: o => o.MapFrom(mapExpression: x => x.Operations))
            .ForAllOtherMembers(memberOptions: x => x.Ignore());

        CreateMap<StockItem, StockItemReadViewModel>()
            .ForMember(destinationMember: x => x.Quantity,
                memberOptions: o => o.MapFrom(mapExpression: x => x.Quantity))
            .ForMember(destinationMember: x => x.GarmentModel,
                memberOptions: o => o.MapFrom(mapExpression: x => x.GarmentModel));
    }
}
