namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.DatabaseInitialization
{
    /// <summary>
    /// Database Initializer
    /// </summary>
    public static class DatabaseInitializer
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // It should be uncomment when using UseSqlServer() settings or any other providers.
            // This is should not be used when UseInMemoryDatabase()
            // await context!.Database.MigrateAsync();

            

            await context.SaveChangesAsync();
        }
    }
}
