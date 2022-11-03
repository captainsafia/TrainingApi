using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

public static class IEndpointConventionBuilderExtensions
{
    public static TBuilder EnableOpenApiWithAuthentication<TBuilder>(this TBuilder builder) where TBuilder : IEndpointConventionBuilder
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
        builder.WithOpenApi(operation => new(operation)
        {
            Security =
            {
                new()
                {
                    [scheme] = new List<string>()
                }
            }
        });
        return builder;
    }
}