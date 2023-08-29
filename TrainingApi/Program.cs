using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using TrainingApi.Shared; 

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorizationBuilder().AddPolicy("trainer_access", policy =>
    policy.RequireRole("trainer").RequireClaim("permission", "admin"));

builder.Services.AddScoped<TrainingService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

#if DEBUG
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<SwaggerGeneratorOptions>(opts => {
    opts.InferSecuritySchemes = true;
});
#endif

var app = builder.Build();

#if DEBUG
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endif

DataGenerator.InitializeDatabase();

var clients = app.MapGroup("/clients/{id}")
    .AddEndpointFilterFactory((handlerContext, next) => {
        var loggerFactory = handlerContext.ApplicationServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("RequestAuditor");
        return (invocationContext) => {
            logger.LogInformation($"[⚙️] Received a request for: {invocationContext.HttpContext.Request.Path}");
            return next(invocationContext);
        };
    });

clients.MapGet("", (int id, TrainingService service) => service.GetClientById(id));
clients.MapPut("", (int id, Client updatedClient, TrainingService service)
    => service.UpdateClientById(id, updatedClient))
.AddEndpointFilter(async (context, next) => {
    var client = context.GetArgument<Client>(1);
    if (client.FirstName.Any(char.IsDigit) || client.LastName.Any(char.IsDigit))
    {
        return Results.Problem("Names cannot contain any numeric characters.", statusCode: 400);
    }
    return await next(context);
});

var trainers = app.MapGroup("/trainers")
#if DEBUG
    .RequireAuthorization("trainer_access")
    .EnableOpenApiWithAuthentication();
#else
    .RequireAuthorization("trainer_access");
#endif

trainers.MapGet("/", (TrainingService service) => service.GetTrainers());
trainers.MapPut("/{id}", (int id, Trainer updatedTrainer, TrainingService service) =>
    service.UpdateTrainerById(id, updatedTrainer));
trainers.MapDelete("/{id}", (int id, TrainingService service) => service.DeleteTrainerById(id));
trainers.MapPost("/", (TrainingService service, Trainer trainer) => service.CreateTrainer(trainer));

app.Run();

[JsonSerializable(typeof(Client))]
[JsonSerializable(typeof(Trainer))]
[JsonSerializable(typeof(List<Trainer>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}