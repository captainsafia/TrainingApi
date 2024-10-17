using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TrainingApi.Apis;
using TrainingApi.Services;
using TrainingApi.Shared; 

var builder = WebApplication.CreateBuilder(args);

// Data and API dependencies
builder.Services
    .AddDbContext<TrainingDb>(options => options.UseInMemoryDatabase("training"));
builder.Services.AddScoped<TrainersService>();
builder.Services.AddScoped<ClientsService>();
// Authentication and authorization dependencies
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorizationBuilder().AddPolicy("trainer_access", policy =>
    policy.RequireRole("trainer").RequireClaim("permission", "admin"));
// OpenAPI dependencies
builder.Services.AddOpenApi(options =>
{
    options.UseJwtBearerAuthentication();
    options.UseExamples();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Set up OpenAPI-related endpoints
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.DefaultFonts = false;
    });
    // Seed the database with mock data
    app.InitializeDatabase();
}

// Redirect for OpenAPI view
app.MapGet("/", () => Results.Redirect("/scalar/v1"))
    .ExcludeFromDescription();
// Register /client and /trainer APIs
app.MapClientApis();
app.MapTrainerApis();

app.Run();
