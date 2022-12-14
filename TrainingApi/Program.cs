using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using TrainingApi.Shared; 

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<TrainingDb>(options => options.UseInMemoryDatabase("training"));

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorizationBuilder().AddPolicy("trainer_access", policy =>
    policy.RequireRole("trainer").RequireClaim("permission", "admin"));

builder.Services.AddScoped<TrainingService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerOptions =>
{
    swaggerOptions.OperationFilter<XmlOperationFilter>();
});
builder.Services.Configure<SwaggerGeneratorOptions>(opts => {
    opts.InferSecuritySchemes = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.InitializeDatabase();

var clients = app.MapGroup("/clients/{id}")
    .AddEndpointFilterFactory((handlerContext, next) => {
        var loggerFactory = handlerContext.ApplicationServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("RequestAuditor");
        return (invocationContext) => {
            logger.LogInformation($"[⚙️] Received a request for: {invocationContext.HttpContext.Request.Path}");
            return next(invocationContext);
        };
    })
    .WithOpenApi();

clients.MapGet("", (int id, TrainingService service) => service.GetClientById(id));
clients.MapPut("", (int id, Client updatedClient, TrainingService service)
    => service.UpdateClientById(id, updatedClient))
.AddEndpointFilter(async (context, next) => {
    var client = context.GetArgument<Client>(2);
    if (client.FirstName.Any(char.IsDigit) || client.LastName.Any(char.IsDigit))
    {
        return Results.Problem("Names cannot contain any numeric characters.", statusCode: 400);
    }
    return await next(context);
});

var trainers = app.MapGroup("/trainers")
    .RequireAuthorization("trainer_access")
    .EnableOpenApiWithAuthentication();

trainers.MapGet("/", (TrainingService service) => service.GetTrainers());
trainers.MapPut("/{id}", (int id, Trainer updatedTrainer, TrainingService service) =>
    service.UpdateTrainerById(id, updatedTrainer));
trainers.MapDelete("/{id}", (int id, TrainingService service) => service.DeleteTrainerById(id));
trainers.MapPost("/", (TrainingService service, Trainer trainer) => service.CreateTrainer(trainer));

app.Run();
