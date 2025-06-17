using Calabonga.AspNetCore.AppDefinitions;
using MassTransit;

namespace SewingFactory.Backend.WarehouseManagement.Web.Definitions.MassTransit;

public sealed class MassTransitDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder) => builder.Services.AddMassTransit(x =>
    {
        x.AddConsumers(typeof(Program).Assembly);
            
        x.AddConfigureEndpointsCallback((name, cfg) =>
        {
            if (cfg is IRabbitMqReceiveEndpointConfigurator rmq)
            {
                rmq.SetQueueArgument("x-dead-letter-exchange", "global.deadletter.exchange"); 
            }
            cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(1))); 
            cfg.UseDelayedRedelivery(r => r.Intervals(
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5),
                TimeSpan.FromMinutes(15)
            )); 
        });
            
        x.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host("localhost", "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
            cfg.ConfigureEndpoints(ctx);
        });;
    });
}
