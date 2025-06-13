using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations.Base;
using SewingFactory.Backend.WarehouseManagement.Web.Extensions;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.Operations;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Mapping.Profiles;

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
