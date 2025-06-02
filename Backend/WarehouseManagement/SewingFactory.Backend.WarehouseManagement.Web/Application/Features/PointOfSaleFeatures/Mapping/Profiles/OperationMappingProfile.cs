using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Base;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Operations;
using SewingFactory.Backend.WorkshopManagement.Web.Extensions;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Mapping.Profiles;

public class OperationMappingProfile : AutoMapper.Profile
{
    public OperationMappingProfile()
    {
        CreateMap<Operation, ReadOperationViewModel>()
            .ForMember(destinationMember: x => x.Date,
                memberOptions: o => o.MapFrom(mapExpression: x => x.Date))
            .ForMember(destinationMember: x => x.Id, memberOptions: o => o.MapFrom(mapExpression: x => x.Id))
            .ForMember(destinationMember: x => x.Quantity, memberOptions: o => o.MapFrom(mapExpression: x => x.Quantity));

        CreateMap<WriteOffOperation, ReadWriteOffOperationViewModel>()
            .IncludeBase<Operation, ReadOperationViewModel>();

        CreateMap<ReceiveOperation, ReadReceiveOperationViewModel>()
            .IncludeBase<Operation, ReadOperationViewModel>();

        CreateMap<SaleOperation, ReadSellOperationViewModel>()
            .IncludeBase<Operation, ReadOperationViewModel>();

        CreateMap<InternalTransferOperation, ReadInternalTransferOperationViewModel>()
            .IncludeBase<Operation, ReadOperationViewModel>()
            .ForMember(destinationMember: x => x.PointOfSale,
                memberOptions: o => o.MapFrom(mapExpression: x => x.Receiver))
            .ForAllOtherMembers(memberOptions: x => x.Ignore());
    }
}
