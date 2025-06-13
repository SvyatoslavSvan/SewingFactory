using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.ViewModels;
using System.Text.Json.Serialization;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.Operations;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ReadWriteOffOperationViewModel), "writeOff")]
[JsonDerivedType(typeof(ReadSellOperationViewModel), "sell")]
[JsonDerivedType(typeof(ReadReceiveOperationViewModel), "receive")]
[JsonDerivedType(typeof(ReadInternalTransferOperationViewModel), "internalTransfer")]
public class ReadOperationViewModel : IIdentityViewModel
{
    public int Quantity { get; set; }
    public DateOnly Date { get; set; }
    public Guid Id { get; set; }
}

public class ReadReceiveOperationViewModel : ReadOperationViewModel
{
}

public class ReadSellOperationViewModel : ReadOperationViewModel
{
}

public class ReadWriteOffOperationViewModel : ReadOperationViewModel
{
}

public class ReadInternalTransferOperationViewModel : ReadOperationViewModel
{
    public required PointOfSaleReadViewModel PointOfSale { get; set; }
}
