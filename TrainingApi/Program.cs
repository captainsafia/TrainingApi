using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TrainingApi.Shared;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Data and API dependencies
builder.Services
    .AddDbContext<TrainingDb>(options => options.UseInMemoryDatabase("training"));
builder.Services.AddScoped<TrainingService>();
// Authentication and authorization dependencies
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorizationBuilder().AddPolicy("trainer_access", policy =>
    policy.RequireRole("trainer").RequireClaim("permission", "admin"));
// OpenAPI dependencies
builder.Services.AddOpenApi(options => options.AddJwtBearerSupport());
// Configure JSON serializer context for native AoT
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Set up OpenAPI-related endpoints
    app.MapOpenApi();
    if (RuntimeFeature.IsDynamicCodeSupported)
    {
        app.MapScalarApiReference();
    }
    // Seed the database with mock data
    app.InitializeDatabase();
}

// Redirect for OpenAPI view
app.MapGet("/", () => TypedResults.Redirect("/scalar/v1"))
    .ExcludeFromDescription();
app.MapTrainerApi();

app.Run();

[JsonSerializable(typeof(Trainer))]
[JsonSerializable(typeof(Client))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}