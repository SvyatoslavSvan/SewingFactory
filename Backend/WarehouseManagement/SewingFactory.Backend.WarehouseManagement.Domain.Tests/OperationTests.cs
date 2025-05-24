using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Tests;

/// <summary>
///     Unit tests covering <see cref="Operation" /> setters.
/// </summary>
public sealed class OperationTests
{
    [Fact(DisplayName = "Quantity setter rejects negative values")]
    public void Quantity_Negative_Throws()
    {
        var (pos, _, _) = TestFixture.CreatePOS();
        var op = new Operation(pos, 1,
            DateOnly.FromDateTime(DateTime.Today), OperationType.Sale);

        Assert.Throws<SewingFactoryArgumentException>(testCode: () => op.Quantity = -5);
    }

    [Fact(DisplayName = "PointOfSale setter rejects null")]
    public void PointOfSale_Null_Throws()
    {
        var (pos, _, _) = TestFixture.CreatePOS();
        var op = new Operation(pos, 1,
            DateOnly.FromDateTime(DateTime.Today), OperationType.Sale);

        Assert.Throws<SewingFactoryArgumentNullException>(testCode: () => op.Owner = null!);
    }
}
