using Calabonga.AspNetCore.AppDefinitions;
using Serilog;
using Serilog.Events;

try
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();

    
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(configureLogger: (context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    builder.AddDefinitions(typeof(Program));

    
    var app = builder.Build();

    
    app.UseDefinitions();

    
    app.UseSerilogRequestLogging();

    
    app.Run();

    return 0;
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (type.Equals("HostAbortedException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, "Unhandled exception");

    throw;
}
finally
{
    Log.CloseAndFlush();
}