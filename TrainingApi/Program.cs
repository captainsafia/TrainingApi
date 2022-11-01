using Microsoft.EntityFrameworkCore;
using TrainingApi.Shared; 

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<TrainingDb>(options => options.UseInMemoryDatabase("training"));

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddScoped<TrainingService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.InitializeDatabase();

app.MapGet("/clients/{id}", (int id, TrainingService service) => service.GetClientById(id))
    .AddEndpointFilterFactory(RequestAuditor)
    .WithOpenApi();

app.MapPut("/clients/{id}", (int id, Client updatedClient, TrainingService service) =>
    {
        return service.UpdateClientById(id, updatedClient);
    })
    .AddEndpointFilterFactory(RequestAuditor)
    .WithOpenApi()
    .AddEndpointFilter(async (context, next) =>
    {
        var client = context.GetArgument<Client>(2);
        if (client.FirstName.Any(char.IsDigit) || client.LastName.Any(char.IsDigit))
        {
            return Results.Problem("Names cannot contain any numeric characters.", statusCode: 400);
        }
        return await next(context);
    });

app.MapGet("/trainers", (TrainingService service) => service.GetTrainers())
    .RequireAuthorization()
    .WithOpenApi();

app.MapPut("/trainers/{id}", (int id, Trainer updatedTrainer, TrainingService service) =>
    {
        return service.UpdateTrainerById(id, updatedTrainer);
    })
    .WithOpenApi()
    .RequireAuthorization(p => p.RequireClaim("is_elite", "true"));

app.MapDelete("/trainers/{id}", (int id, TrainingService service) => service.DeleteTrainerById(id))
    .RequireAuthorization()
    .WithOpenApi();

app.MapPost("/trainers", (TrainingService service, Trainer trainer) => service.CreateTrainer(trainer))
    .RequireAuthorization()
    .WithOpenApi();

app.Run();

static EndpointFilterDelegate RequestAuditor(EndpointFilterFactoryContext handlerContext, EndpointFilterDelegate next)
{
    var loggerFactory = handlerContext.ApplicationServices.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("RequestAuditor");
    return (invocationContext) =>
    {
        logger.LogInformation($"[⚙️] Received a request for: {invocationContext.HttpContext.Request.Path}");
        return next(invocationContext);
    };
};
