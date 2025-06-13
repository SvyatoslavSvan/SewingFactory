using ClosedXML.Excel;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations.Base;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Interfaces;
using SewingFactory.Common.Domain.ValueObjects;
using System.Globalization;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Implementations;

public sealed class AllOperationForPointOfSaleReportProvider : IAllOperationForPointOfSaleReportProvider
{
    public byte[] Build(PointOfSale pointOfSale, DateRange period)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Реєстр");

        var row = 1;
        WriteReportHeader(ws, ref row, pointOfSale, period);
        WriteTableHeader(ws, ref row);
        var (totalIncome, totalOutcome) = WriteBody(ws, ref row, pointOfSale, period);
        WriteTotals(ws, ref row, totalIncome, totalOutcome);

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }

    private static void WriteReportHeader(
        IXLWorksheet ws, ref int row,
        PointOfSale pos, DateRange period)
    {
        ws.Cell(row, 1)
                .Value = $"Реєстр з торгової точки \"{pos.Name}\"";
        ws.Range(row, 1, row, 5).Merge()
            .Style.Font.SetBold().Font.SetFontSize(14);
        row++;

        ws.Cell(row, 1)
                .Value = $"За період з {RuDate(period.Start)} по {RuDate(period.End)}";
        ws.Range(row, 1, row, 5).Merge()
            .Style.Font.SetBold();
        row += 2;
    }

    private static void WriteTableHeader(IXLWorksheet ws, ref int row)
    {
        ws.Cell(row, 1).Value = "Дата";
        ws.Cell(row, 2).Value = "Тип операції";
        ws.Cell(row, 3).Value = "Отримано";
        ws.Cell(row, 4).Value = "Витрачено";
        ws.Cell(row, 5).Value = "Контрагент";

        ws.Range(row, 1, row, 5).Style.Font.SetBold();
        row++;
    }

    private static (decimal income, decimal outcome) WriteBody(
        IXLWorksheet ws, ref int row, PointOfSale pos, DateRange period)
    {
        decimal incomeSum = 0, outcomeSum = 0;

        var groups = pos.Operations
            .Where(o => o.Date >= period.Start && o.Date <= period.End)
            .Select(o => new
            {
                o.Date,
                o.DisplayName,
                Receiver = o is InternalTransferOperation ito ? ito.Receiver.Name : null,
                o.HistoricalSum,
                IsIncome = o is ReceiveOperation
            })
            .GroupBy(x => new { x.Date, x.DisplayName, x.Receiver, x.IsIncome })
            .Select(g => new
            {
                g.Key.Date,
                g.Key.DisplayName,
                g.Key.Receiver,
                g.Key.IsIncome,
                Sum = g.Sum(x => x.HistoricalSum)
            })
            .OrderBy(x => x.Date)
            .ThenBy(x => x.DisplayName);

        foreach (var item in groups)
        {
            ws.Cell(row, 1).Value = item.Date.ToDateTime(TimeOnly.MinValue)
                .ToString("dd.MM.yyyy");
            ws.Cell(row, 2).Value = item.DisplayName;

            if (item.IsIncome)
            {
                ws.Cell(row, 3).Value = item.Sum;
                incomeSum += item.Sum;
            }
            else
            {
                ws.Cell(row, 4).Value = item.Sum;
                outcomeSum += item.Sum;
            }

            if (!string.IsNullOrWhiteSpace(item.Receiver))
            {
                ws.Cell(row, 5).Value = item.Receiver;
            }

            row++;
        }

        ws.Range(4, 3, row - 1, 4).Style.NumberFormat.SetFormat("#,##0.00");
        return (incomeSum, outcomeSum);
    }
    
    private static void WriteTotals(
        IXLWorksheet ws, ref int row, decimal income, decimal outcome)
    {
        row++; 

        ws.Cell(row, 1).Value = "Разом:";
        ws.Cell(row, 1).Style.Font.SetBold();

        ws.Cell(row, 3).Value = income;
        ws.Cell(row, 4).Value = outcome;

        ws.Range(row, 3, row, 4)
            .Style.NumberFormat.SetFormat("#,##0.00")
            .Font.SetBold();
        row++;
    }

    private static bool IsIncome(Operation op) =>
        op is ReceiveOperation;

    private static string RuDate(DateOnly date) =>
        date.ToDateTime(TimeOnly.MinValue)
            .ToString("dd MMMM yyyy 'г.'", new CultureInfo("ru-RU"));
}
