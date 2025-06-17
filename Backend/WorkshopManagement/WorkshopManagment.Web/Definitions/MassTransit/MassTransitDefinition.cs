using Calabonga.AspNetCore.AppDefinitions;
using MassTransit;
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
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });;
        });
    }
}