
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Authorization;
using TrainingApi.Shared;
using Microsoft.OpenApi.Any;

public static class OpenApiTransformersExtensions
{
    public static OpenApiOptions UseJwtBearerAuthentication(this OpenApiOptions options)
    {
        var scheme = new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.Http,
            Name = JwtBearerDefaults.AuthenticationScheme,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Reference = new()
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            }
        };
        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Components ??= new();
            document.Components.SecuritySchemes.Add(JwtBearerDefaults.AuthenticationScheme, scheme);
            return Task.CompletedTask;
        });
        options.AddOperationTransformer((operation, context, cancellationToken) =>
        {
            if (context.Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>().Any())
            {
                operation.Security = [new() { [scheme] = [] }];
            }
            return Task.CompletedTask;
        });
        return options;
    }

    public static OpenApiOptions UseExamples(this OpenApiOptions options)
    {
        options.AddSchemaTransformer((schema, context, cancellationToken) =>
        {
            if (context.JsonTypeInfo.Type == typeof(Trainer))
            {
                schema.Example = new OpenApiObject
                {
                    ["id"] = new OpenApiInteger(1),
                    ["firstName"] = new OpenApiString("John"),
                    ["lastName"] = new OpenApiString("Doe"),
                    ["email"] = new OpenApiString("john.doe@email.com"),
                    ["level"] = new OpenApiString("Junior"),
                    ["isCertificationActive"] = new OpenApiBoolean(false)
                };
            }
            if (context.JsonTypeInfo.Type == typeof(Client))
            {
                schema.Example = new OpenApiObject
                {
                    ["id"] = new OpenApiInteger(1),
                    ["firstName"] = new OpenApiString("Jane"),
                    ["lastName"] = new OpenApiString("Smith"),
                    ["email"] = new OpenApiString("jane.smith@email.com"),
                    ["weight"] = new OpenApiInteger(60),
                    ["height"] = new OpenApiInteger(170),
                    ["birthDate"] = new OpenApiDateTime(new DateTime(1990, 1, 1))
                };
            }
            return Task.CompletedTask;
        });
        return options;
    }
}