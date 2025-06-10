using ClosedXML.Excel;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Implementations;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Tests;

public sealed class AllOperationForStockReportProviderTests
{
    private static Money Amount(decimal amount) => new(amount);

    private static (PointOfSale pos, PointOfSale receiver,
        GarmentModel model, DateRange range,
        decimal expectedTotal, int expectedRows) Arrange()
    {
        var cat = new GarmentCategory("Футболки", []);
        var model = new GarmentModel("F-100", cat, Amount(250));

        var posSender = new PointOfSale("ТТ-1");
        var posReceiver = new PointOfSale("ТТ-2");
        posSender.Receive(model, 20, new DateOnly(2025, 6, 1));
        posSender.Sell(model.Id, 3, new DateOnly(2025, 6, 5)); //  750
        posSender.WriteOff(model.Id, 1, new DateOnly(2025, 6, 5)); //  250
        posSender.Receive(model, 5, new DateOnly(2025, 6, 6)); // 1250
        posSender.Transfer(model, 2, new DateOnly(2025, 6, 7), posReceiver); // 500

        var range = new DateRange(new DateOnly(2025, 6, 5), new DateOnly(2025, 6, 7));
        var total = 750m + 250m + 1250m + 500m; // 2 750
        var rowsInBody = 4; 

        return (posSender, posReceiver, model, range, total, rowsInBody);
    }
    [Fact]
    public void WorksheetName_ShouldBe_Reestr()
    {
        var (pos, _, model, range, _, _) = Arrange();
        var sut = new AllOperationForStockReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(pos, model, range)));
        Assert.Single(wb.Worksheets);
        Assert.Equal("Реєстр", wb.Worksheet(1).Name);
    }

    [Fact]
    public void Header_ShouldContain_Model_PointOfSale_And_Period()
    {
        var (pos, _, model, range, _, _) = Arrange();
        var sut = new AllOperationForStockReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(pos, model, range)));
        var ws = wb.Worksheet(1);

        Assert.Equal($"Реєстр з продукції \"{model.Name}\"", ws.Cell(1, 1).GetString());
        Assert.Equal($"по ТТ \"{pos.Name}\"", ws.Cell(2, 1).GetString());
        Assert.Equal($"За період з {range.Start:dd MMM yyyy} по {range.End:dd MMM yyyy}",
            ws.Cell(3, 1).GetString());
    }

    [Fact]
    public void ColumnHeaders_ShouldMatch_Specification()
    {
        var (pos, _, model, range, _, _) = Arrange();
        var sut = new AllOperationForStockReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(pos, model, range)));
        var ws = wb.Worksheet(1);

        string[] expected = { "Дата", "Вид", "К-сть", "Ціна", "Сумма", "Від кого", "Кому" };

        for (var i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], ws.Cell(5, i + 1).GetString());
        }
    }

    [Fact]
    public void Body_ShouldHave_CorrectRows_And_Total()
    {
        var (pos, receiver, model, range, expectedTotal, expectedRows) = Arrange();
        var sut = new AllOperationForStockReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(pos, model, range)));
        var ws = wb.Worksheet(1);
        const int firstDataRow = 6; 
        var usedRows = ws.RangeUsed()!.RowsUsed().ToList();
        var totalRowNo = usedRows.Last().RowNumber(); 
        var bodyRows = usedRows.Where(predicate: r => r.RowNumber() >= firstDataRow &&
                                                      r.RowNumber() < totalRowNo)
            .ToList();

        Assert.Equal(expectedRows, bodyRows.Count);
        var sum = bodyRows.Sum(selector: r => r.Cell(5).GetValue<decimal?>() ?? 0m);
        Assert.Equal(expectedTotal, sum);
        var rowWithPartners = bodyRows.Single(predicate: r =>
            !string.IsNullOrWhiteSpace(r.Cell(6).GetString()) &&
            !string.IsNullOrWhiteSpace(r.Cell(7).GetString()));

        Assert.Equal(pos.Name, rowWithPartners.Cell(6).GetString());
        Assert.Equal(receiver.Name, rowWithPartners.Cell(7).GetString());
    }

    [Fact]
    public void TotalsRow_ShouldContain_Label_And_Formula()
    {
        var (pos, _, model, range, _, expectedRows) = Arrange();
        var sut = new AllOperationForStockReportProvider();

        using var wb = new XLWorkbook(new MemoryStream(sut.Build(pos, model, range)));
        var ws = wb.Worksheet(1);
        int totalRow = 5 /*header*/ + 1 /*перша data-row*/ + expectedRows;
        Assert.Equal("Разом:", ws.Cell(totalRow, 4).GetString());
        string expectedFormula = $"SUM(E6:E{totalRow - 1})";
        Assert.Equal(expectedFormula, ws.Cell(totalRow, 5).FormulaA1);
    }
}
