using Calabonga.AspNetCore.AppDefinitions;
using MassTransit;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Publisher;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Publisher;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.MassTransit;

public sealed class MassTransitDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IGarmentCategoryPublisher, GarmentCategoryPublisher>();
        builder.Services.AddScoped<IGarmentModelPublisher, GarmentModelPublisher>();
        builder.Services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<ApplicationDbContext>(o =>
            {
                o.QueryDelay = TimeSpan.FromSeconds(10);
                o.UsePostgres();
                o.UseBusOutbox();
            });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            ;
        });
    }
}