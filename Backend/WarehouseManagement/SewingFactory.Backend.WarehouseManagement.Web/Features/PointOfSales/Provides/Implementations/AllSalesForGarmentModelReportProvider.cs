using ClosedXML.Excel;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Interfaces;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Implementations;

public sealed class AllSalesForGarmentModelReportProvider : IAllSalesForGarmentModelReportProvider
{
    public byte[] Build(IList<SaleOperation> sales, DateRange dateRange)
    {
        if (sales == null || !sales.Any())
        {
            throw new ArgumentException("No sales provided", nameof(sales));
        }

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Report");

        var modelName = sales.First().StockItem.GarmentModel.Name;
        WriteHeader(worksheet, modelName, dateRange);
        WriteColumnTitles(worksheet);
        WriteDataRows(worksheet, sales);
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    private static void WriteHeader(IXLWorksheet ws, string modelName, DateRange dateRange)
    {
        ws.Cell(1, 1).Value = $"Продажі по продукції \"{modelName}\"";
        var header = ws.Range(1, 1, 1, 4).Merge();
        
        header.Style.Font.SetBold();
        header.Style.Font.SetFontSize(14);
        header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        header.Style.Alignment.Vertical   = XLAlignmentVerticalValues.Center;
        header.Style.Alignment.WrapText   = false;    
        header.Style.Alignment.ShrinkToFit = true;   
        ws.Row(1).AdjustToContents(14, 100);
        ws.Cell(2, 1).Value = $"За період з {dateRange.Start:dd MMM yyyy} по {dateRange.End:dd MMM yyyy}";
        var subHeader = ws.Range(2, 1, 2, 4).Merge();

        subHeader.Style.Font.SetBold();
        subHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        subHeader.Style.Alignment.Vertical   = XLAlignmentVerticalValues.Center;
        subHeader.Style.Alignment.WrapText   = false;
        subHeader.Style.Alignment.ShrinkToFit = true;
        ws.Row(2).AdjustToContents(14, 100);
        
        ws.Columns(1, 4).AdjustToContents();
    }

    private static void WriteColumnTitles(IXLWorksheet ws)
    {
        var titles = new[] { "Дата", "Кількість", "Ціна", "Сума" };
        for (var i = 0; i < titles.Length; i++)
        {
            ws.Cell(4, i + 1).Value = titles[i];
            ws.Cell(4, i + 1).Style.Font.SetBold();
            ws.Cell(4, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Column(i + 1).Width = 15;
        }
    }

    private static void WriteDataRows(IXLWorksheet ws, IList<SaleOperation> sales)
    {
        var row = 5;

        var groups = sales
            .OrderBy(s => s.Owner.Name)
            .ThenBy(s => s.Date)
            .GroupBy(s => s.Owner.Name);

        foreach (var grp in groups)
        {
            var groupHeader = ws.Range(row, 1, row, 4).Merge();
            groupHeader.Value = grp.Key;
            groupHeader.Style.Font.SetBold();
            groupHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            groupHeader.Style.Alignment.Vertical   = XLAlignmentVerticalValues.Center;

            row++;

            decimal totalSum = 0;

            foreach (var sale in grp)
            {
                ws.Cell(row, 1).Value = sale.Date.ToString("dd.MM.yy");
                ws.Cell(row, 2).Value = sale.Quantity;
                ws.Cell(row, 3).Value = sale.PriceOnOperationDate.Amount;
                ws.Cell(row, 4).Value = sale.HistoricalSum;
                totalSum += sale.HistoricalSum;
                row++;
            }

            ws.Range(row, 1, row, 3).Merge()
                .SetValue("Разом:")
                .Style
                .Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            ws.Cell(row, 4)
                .SetValue(totalSum)
                .Style
                .NumberFormat.SetFormat("# ##0.00")
                .Font.SetBold();

            row += 2; 
        }
    }
}
