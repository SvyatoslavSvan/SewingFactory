using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.DatabaseInitialization;

/// <summary>
///     Database Initializer
/// </summary>
public static class DatabaseInitializer
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        await using var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (await db.Departments.AnyAsync())
        {
            return;
        }

        #region Departments

        var cutting = new Department("Розкрій");
        var sewing = new Department("Швейний цех");
        var packaging = new Department("Пакування");
        var technology = new Department("Технологічний відділ");

        await db.Departments.AddRangeAsync(cutting, sewing, packaging, technology);

        #endregion

        #region Processes

        var cutCloth = new Process("Розкрій тканини", cutting, new Money(5));
        var overlock = new Process("Оверлок", sewing, new Money(8));
        var seam = new Process("Основна строчка", sewing, new Money(10));
        var qc = new Process("Контроль якості", technology, new Money(4));
        var packProc = new Process("Пакування", packaging, new Money(3));

        await db.Processes.AddRangeAsync(cutCloth, overlock, seam, qc, packProc);

        #endregion

        #region Garment categories / models

        var teeCat = new GarmentCategory("Футболки");
        var jeansCat = new GarmentCategory("Джинси");

        await db.GarmentCategories.AddRangeAsync(teeCat, jeansCat);

        var tShirt = new GarmentModel(
            "Футболка Classic",
            "Класична бавовняна футболка",
            [cutCloth, overlock, seam, packProc],
            teeCat,
            new Money(20));

        var jeans = new GarmentModel(
            "Джинси Slim",
            "Джинси облягаючого фасону",
            [cutCloth, seam, qc, packProc],
            jeansCat,
            new Money(35));

        await db.GarmentModels.AddRangeAsync(tShirt, jeans);

        #endregion

        #region Employees

        var john = new ProcessBasedEmployee("Іван Кравець", "EMP001", cutting, new Percent(10));
        var anna = new ProcessBasedEmployee("Анна Швачка", "EMP002", sewing, new Percent(5));
        var mike = new RateBasedEmployee("Михайло Пакувальник", "EMP003", new Money(7000), packaging, 5);
        var kate = new Technologist("Катерина Технолог", "EMP004", new Percent(3), technology);

        cutting.AddEmployee(john);
        sewing.AddEmployee(anna);
        packaging.AddEmployee(mike);
        technology.AddEmployee(kate);

        await db.ProcessBasedEmployees.AddRangeAsync(john, anna);
        await db.RateBasedEmployees.AddAsync(mike);
        await db.Technologists.AddAsync(kate);

        #endregion

        #region Workshop document (із завданнями та повтореннями)

        var document = WorkshopDocument.CreateInstance(
            "Замовлення #1 – Футболка Classic",
            100,
            DateOnly.FromDateTime(DateTime.UtcNow.Date),
            tShirt,
            sewing);

        foreach (var task in document.Tasks)
        {
            if (task.Process == cutCloth)
            {
                task.AddEmployeeRepeat(new EmployeeTaskRepeat(john, 100));
            }
            else if (task.Process == overlock || task.Process == seam)
            {
                task.AddEmployeeRepeat(new EmployeeTaskRepeat(anna, 100));
            }
            else if (task.Process == packProc)
            {
                task.AddEmployeeRepeat(new EmployeeTaskRepeat(mike, 100));
            }
        }

        await db.WorkshopDocuments.AddAsync(document);

        #endregion

        #region Timesheet для Rate-based співробітника

        var timesheet = Timesheet.CreateInstance(
            [mike],
            new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1));

        await db.Timesheets.AddAsync(timesheet);

        #endregion

        await db.SaveChangesAsync();
    }
}