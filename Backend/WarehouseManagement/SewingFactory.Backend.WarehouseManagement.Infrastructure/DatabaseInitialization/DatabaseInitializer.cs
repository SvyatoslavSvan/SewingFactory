using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.DatabaseInitialization;

/// <summary>
///     Database Initializer
/// </summary>
public static class DatabaseInitializer
{
    public static async void Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        await using var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (await db.GarmentCategories.AnyAsync())
        {
            return;
        }
        var tshirtModels = new List<GarmentModel>();
        var tshirtCategory = new GarmentCategory("Футболки", tshirtModels);

        var basicTee   = new GarmentModel("Базова футболка",   tshirtCategory, new Money(350m));
        var premiumTee = new GarmentModel("Преміум‑футболка",  tshirtCategory, new Money(550m));
        tshirtModels.AddRange([basicTee, premiumTee]);

        var jeansModels   = new List<GarmentModel>();
        var jeansCategory = new GarmentCategory("Джинси", jeansModels);
        var classicJeans  = new GarmentModel("Класичні джинси", jeansCategory, new Money(899m));
        jeansModels.Add(classicJeans);

        var jacketModels   = new List<GarmentModel>();
        var jacketCategory = new GarmentCategory("Куртки", jacketModels);
        var bomberJacket   = new GarmentModel("Бомбер", jacketCategory, new Money(1850m));
        jacketModels.Add(bomberJacket);
        var mainStore = new PointOfSale("Головний магазин");
        var webStore  = new PointOfSale("Інтернет‑магазин");
        var today       = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday   = today.AddDays(-1);
        var twoDaysAgo  = today.AddDays(-2);
        mainStore.Receive(basicTee,   100, today);
        mainStore.Receive(premiumTee,  50, today);
        mainStore.Receive(classicJeans, 75, today);
        mainStore.Receive(bomberJacket, 20, today);
        webStore.Receive(basicTee,     30, today);
        webStore.Receive(classicJeans, 20, today);
        mainStore.Sell(basicTee.Id,   5, yesterday);
        mainStore.Sell(premiumTee.Id, 3, yesterday);
        webStore.Sell(basicTee.Id,    2, yesterday);

        mainStore.WriteOff(premiumTee.Id, 1, yesterday);           
        webStore.WriteOff(basicTee.Id,    1, twoDaysAgo);          

        mainStore.Transfer(classicJeans, 10, twoDaysAgo, webStore); 

        db.GarmentCategories.AddRange(tshirtCategory, jeansCategory, jacketCategory);
        db.PointOfSales.AddRange(mainStore, webStore);
        await db.SaveChangesAsync();
    }
}
