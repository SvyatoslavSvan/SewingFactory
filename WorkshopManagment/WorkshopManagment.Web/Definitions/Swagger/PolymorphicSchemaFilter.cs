using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.Swagger;

public class PolymorphicSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext ctx)
    {
        var polyAttr = ctx.Type.GetCustomAttribute<JsonPolymorphicAttribute>();
        if (polyAttr is null)
        {
            return;
        }

        var derivedAttrs = ctx.Type.GetCustomAttributes<JsonDerivedTypeAttribute>().ToList();
        if (derivedAttrs.Count == 0)
        {
            return;
        }

        var repo = ctx.SchemaRepository;
        var gen = ctx.SchemaGenerator;

        var oneOfSchemas = new List<OpenApiSchema>();

        var mapping = new Dictionary<string, string>();

        foreach (var attr in derivedAttrs)
        {
            var discName = attr.TypeDiscriminator?.ToString()
                           ?? char.ToLower(attr.DerivedType.Name[0]) + attr.DerivedType.Name[1..];

            var derivedSchema = gen.GenerateSchema(attr.DerivedType, repo);

            oneOfSchemas.Add(new OpenApiSchema { Reference = derivedSchema.Reference });

            mapping[discName] = $"#/components/schemas/{derivedSchema.Reference.Id}";
        }

        var discProp = polyAttr.TypeDiscriminatorPropertyName ?? "$type";
        schema.Properties.Add(discProp, new OpenApiSchema
        {
            Type = "string",
            Description = "Discriminator",
            Enum = mapping.Keys
                .Select(selector: d => (IOpenApiAny)new OpenApiString(d))
                .ToList()
        });

        schema.Discriminator = new OpenApiDiscriminator { PropertyName = polyAttr.TypeDiscriminatorPropertyName ?? "$type", Mapping = mapping };

        schema.OneOf = oneOfSchemas;
    }
}