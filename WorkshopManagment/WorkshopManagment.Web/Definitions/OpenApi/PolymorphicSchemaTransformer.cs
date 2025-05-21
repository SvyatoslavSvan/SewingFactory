using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.OpenApi;

public sealed class PolymorphicSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext ctx,
        CancellationToken _)
    {
        var clr = ctx.JsonTypeInfo?.Type;
        if (clr is null)
        {
            return Task.CompletedTask;
        }

        var polyAttr = clr.GetCustomAttribute<JsonPolymorphicAttribute>();
        var derived = clr.GetCustomAttributes<JsonDerivedTypeAttribute>().ToList();
        if (polyAttr is null || derived.Count == 0)
        {
            return Task.CompletedTask;
        }

        var discName = polyAttr.TypeDiscriminatorPropertyName ?? "$type";
        if (!schema.Properties.ContainsKey(discName))
        {
            var enumValues = derived
                .Select(selector: d => d.TypeDiscriminator?.ToString()
                                       ?? char.ToLower(d.DerivedType.Name[0]) + d.DerivedType.Name[1..])
                .Distinct()
                .Select(selector: v => (IOpenApiAny)new OpenApiString(v))
                .ToList();

            schema.Properties[discName] = new OpenApiSchema { Type = "string", Description = "Type discriminator", Enum = enumValues };
        }

        if (schema.AnyOf is { Count: > 0 })
        {
            schema.OneOf = new List<OpenApiSchema>(schema.AnyOf);

            schema.AnyOf = new List<OpenApiSchema>();
        }

        return Task.CompletedTask;
    }
}