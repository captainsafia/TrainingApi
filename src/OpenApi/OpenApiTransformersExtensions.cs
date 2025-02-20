
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Authorization;
using TrainingApi.Shared;
using System.Text.Json.Nodes;
using Microsoft.OpenApi.Models.References;
using Microsoft.OpenApi.Models.Interfaces;

public static class OpenApiTransformersExtensions
{
    public static OpenApiOptions UseJwtBearerAuthentication(this OpenApiOptions options)
    {
        var scheme = new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.Http,
            Name = JwtBearerDefaults.AuthenticationScheme,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
        };
        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Components ??= new();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
            document.Components.SecuritySchemes.Add(JwtBearerDefaults.AuthenticationScheme, scheme);
            return Task.CompletedTask;
        });
        options.AddOperationTransformer((operation, context, cancellationToken) =>
        {
            if (context.Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>().Any())
            {
                operation.Security = [new() {{ new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme), [] }}];
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
                schema.Example = new JsonObject
                {
                    ["id"] = 1,
                    ["firstName"] = "John",
                    ["lastName"] = "Doe",
                    ["email"] = "john.doe@email.com",
                    ["level"] = "Junior",
                    ["isCertificationActive"] = false
                };
            }
            if (context.JsonTypeInfo.Type == typeof(Client))
            {
                schema.Example = new JsonObject
                {
                    ["id"] = 1,
                    ["firstName"] = "Jane",
                    ["lastName"] = "Smith",
                    ["email"] ="jane.smith@email.com",
                    ["weight"] = 60,
                    ["height"] = 170,
                    ["birthDate"] = "1990-01-01"
                };
            }
            return Task.CompletedTask;
        });
        return options;
    }

    public static OpenApiOptions MapType<T>(this OpenApiOptions options, JsonSchemaType type)
    {
        options.AddSchemaTransformer((schema, context, cancellationToken) =>
        {
            if (context.JsonTypeInfo.Type == typeof(T))
            {
                schema.Type = type;
            }
            return Task.CompletedTask;
        });
        
        return options;
    }
}