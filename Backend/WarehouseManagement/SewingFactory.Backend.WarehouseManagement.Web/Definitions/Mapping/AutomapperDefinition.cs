﻿using Calabonga.AspNetCore.AppDefinitions;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace SewingFactory.Backend.WarehouseManagement.Web.Definitions.Mapping;

/// <summary>
///     Register Automapper as MicroserviceDefinition
/// </summary>
public class AutomapperDefinition : AppDefinition
{
    /// <summary>
    ///     Configure services for current microservice
    /// </summary>
    /// <param name="builder"></param>
    public override void ConfigureServices(WebApplicationBuilder builder)
        => builder.Services.AddAutoMapper(typeof(Program));

    /// <summary>
    ///     Configure application for current microservice
    /// </summary>
    /// <param name="app"></param>
    public override void ConfigureApplication(WebApplication app)
    {
        var mapper = app.Services.GetRequiredService<IConfigurationProvider>();
        if (app.Environment.IsDevelopment())
        {
            // validate Mapper Configuration
            mapper.AssertConfigurationIsValid();
        }
        else
        {
            mapper.CompileMappings();
        }
    }
}
