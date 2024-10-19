using Microsoft.EntityFrameworkCore;
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Seed the database with mock data
    app.InitializeDatabase();
}

// Register /client and /trainer APIs
app.MapClientApis();
app.MapTrainerApis();

app.Run();
