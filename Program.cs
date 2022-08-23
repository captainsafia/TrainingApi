using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<TrainingDb>(options => options.UseInMemoryDatabase("training"));

builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

clients.MapGet("/", async (int id, TrainingDb db) =>
{
    var client = await db.Clients.FindAsync(id);
    return client is null ? Results.NotFound() : Results.Ok(client);
})
.Produces<Client>();

clients.MapPut("/", async (int id, TrainingDb db, Client updatedClient) =>
{
    var client = await db.Clients.FindAsync(id);
    if (client is null) return Results.NotFound();
    client = updatedClient;
    await db.SaveChangesAsync();
    return Results.Created($"/clients/{client.Id}", client);
})
.AddEndpointFilter(async (context, next) => {
    var client = context.GetArgument<Client>(2);
    if (client.FirstName.Any(char.IsDigit) || client.LastName.Any(char.IsDigit))
    {
        return Results.Problem("Names cannot contain any numeric characters.", statusCode: 400);
    }
    return await next(context);
});

var trainers = app.MapGroup("/trainers")
    .RequireAuthorization()
    .WithOpenApi();

trainers.MapGet("/", (TrainingDb db) => Results.Ok(db.Trainers));

trainers.MapPut("/{id}", async (int id, TrainingDb db, Trainer updatedTrainer) =>
{
    var trainer = await db.Trainers.FindAsync(id);
    if (trainer is null) return Results.NotFound();
    trainer = updatedTrainer;
    await db.SaveChangesAsync();
    return Results.Created($"/trainers/{trainer.Id}", trainer);
})
.RequireAuthorization(p => p.RequireClaim("is_elite", "true"));

app.Run();
