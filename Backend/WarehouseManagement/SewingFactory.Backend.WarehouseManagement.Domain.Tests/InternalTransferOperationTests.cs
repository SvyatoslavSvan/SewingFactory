using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Tests;

/// <summary>
///     Unit tests covering <see cref="InternalTransferOperation" /> setters.
/// </summary>
public sealed class InternalTransferOperationTests
{
    [Fact(DisplayName = "Receiver setter rejects null")]
    public void Receiver_Null_Throws()
    {
        var (sender, model, _) = TestFixture.CreatePOS();
        var receiver = new PointOfSale("Receiver");

        var op = new InternalTransferOperation(sender, 1,
            DateOnly.FromDateTime(DateTime.Today), receiver);

        Assert.Throws<SewingFactoryArgumentNullException>(testCode: () => op.Receiver = null!);
    }
}
