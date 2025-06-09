using ClosedXML.Excel;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations.Base;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Interfaces;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Implementations;

public sealed class AllOperationForStockReportProvider : IAllOperationForStockReportProvider
{
    private const int _titleRow = 1;
    private const int _subtitleRow = 2;
    private const int _periodRow = 3;
    private const int _headerRow = 5;

    private static Operation[] GetFilteredOperations(PointOfSale pointOfSale, GarmentModel garmentModel, DateRange period) => pointOfSale.Operations
        .Where(predicate: op => op.StockItem.GarmentModel.Id == garmentModel.Id && period.Contains(op.Date))
        .OrderBy(keySelector: op => op.Date)
        .ToArray();

    private static void ConfigureTitle(IXLWorksheet ws, GarmentModel model, PointOfSale pos, DateRange period)
    {
        ws.Cell(_titleRow, 1).Value = $"Реєстр з продукції \"{model.Name}\"";
        ws.Range(_titleRow, 1, _titleRow, 7)
            .Merge()
            .Style
            .Font.SetBold()
            .Font.SetFontSize(14)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        ws.Cell(_subtitleRow, 1).Value = $"по ТТ \"{pos.Name}\"";
        ws.Range(_subtitleRow, 1, _subtitleRow, 7)
            .Merge()
            .Style
            .Font.SetBold()
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        ws.Cell(_periodRow, 1).Value = $"За період з {period.Start:dd MMM yyyy} по {period.End:dd MMM yyyy}";
        ws.Range(_periodRow, 1, _periodRow, 7)
            .Merge()
            .Style
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    }

    private static void ConfigureColumnHeaders(IXLWorksheet ws)
    {
        var headers = new[] { "Дата", "Вид", "К-сть", "Ціна", "Сумма", "Від кого", "Кому" };

        for (var i = 0; i < headers.Length; i++)
        {
            ws.Cell(_headerRow, i + 1).Value = headers[i];
            ws.Cell(_headerRow, i + 1).Style.Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        }
    }

    private static void PopulateRecords(IXLWorksheet ws, Operation[] records)
    {
        var row = _headerRow + 1;
        foreach (var operation in records)
        {
            ws.Cell(row, 1).Value = operation.Date.ToDateTime(TimeOnly.MinValue);
            ws.Cell(row, 1).Style.DateFormat.Format = "dd.MM.yy";

            ws.Cell(row, 2).Value = operation.DisplayName;
            ws.Cell(row, 3).Value = operation.Quantity;

            ws.Cell(row, 4).Value = operation.PriceOnOperationDate.Amount;
            ws.Cell(row, 4).Style.NumberFormat.Format = "# ##0.00";
            
            ws.Cell(row, 5).Value = operation.HistoricalSum;
            ws.Cell(row, 5).Style.NumberFormat.Format = "# ##0.00";

            if (operation is InternalTransferOperation transfer)
            {
                ws.Cell(row, 6).Value = transfer.Owner.Name;
                ws.Cell(row, 7).Value = transfer.Receiver.Name;
            }

            row++;
        }
    }
    
    private static void AddTotalRow(IXLWorksheet ws, int firstDataRow, int lastDataRow)
    {
        var totalRow = lastDataRow + 1;

        ws.Cell(totalRow, 4)
            .SetValue("Разом:")
            .Style.Font.SetBold()
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

        ws.Cell(totalRow, 5)
                .FormulaA1 = $"=SUM(E{firstDataRow}:E{lastDataRow})";

        ws.Cell(totalRow, 5).Style
            .NumberFormat.SetFormat("# ##0.00")
            .Font.SetBold();
    }
    
    public byte[] Build(PointOfSale pointOfSale, GarmentModel garmentModel, DateRange period)
    {
        var records = GetFilteredOperations(pointOfSale, garmentModel, period);
        using var workbook = new XLWorkbook();
        var worksheet = workbook.AddWorksheet("Реєстр");

        ConfigureTitle(worksheet, garmentModel, pointOfSale, period);
        ConfigureColumnHeaders(worksheet);
        PopulateRecords(worksheet, records);
        const int firstDataRow = _headerRow + 1;
        var lastDataRow  = _headerRow + records.Length;
        AddTotalRow(worksheet, firstDataRow, lastDataRow);

        worksheet.Columns().AdjustToContents();
        worksheet.SheetView.FreezeRows(_headerRow);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
