using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Base;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Operations;
using SewingFactory.Backend.WorkshopManagement.Web.Extensions;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Mapping.Profiles
{
    public class OperationMappingProfile : AutoMapper.Profile
    {
        public OperationMappingProfile()
        {
            CreateMap<Operation, ReadOperationViewModel>()
                .ForMember(x => x.Date,
                    o => o.MapFrom(x => x.Date))
                .ForMember(x => x.Id, o => o.MapFrom(x => x.Id))
                .ForMember(x => x.Quantity, o => o.MapFrom(x => x.Quantity));

            CreateMap<WriteOffOperation, ReadWriteOffOperationViewModel>()
                .IncludeBase<Operation, ReadOperationViewModel>();

            CreateMap<ReceiveOperation, ReadReceiveOperationViewModel>()
                .IncludeBase<Operation, ReadOperationViewModel>();

            CreateMap<SaleOperation, ReadSellOperationViewModel>()
                .IncludeBase<Operation, ReadOperationViewModel>();

            CreateMap<InternalTransferOperation, ReadInternalTransferOperationViewModel>()
                .IncludeBase<Operation, ReadOperationViewModel>()
                .ForMember(x => x.PointOfSale,
                    o => o.MapFrom(x => x.Receiver))
                .ForAllOtherMembers(x => x.Ignore());

        }
    }
}
