using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TrainingApi.Shared; 

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Set up OpenAPI-related endpoints
    app.MapOpenApi();
    app.MapScalarApiReference();
    // Seed the database with mock data
    app.InitializeDatabase();
}

// Redirect for OpenAPI view
app.MapGet("/", () => TypedResults.Redirect("/scalar/v1"))
    .ExcludeFromDescription();
app.MapTrainerApi();

app.Run();
