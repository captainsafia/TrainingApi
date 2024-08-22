using TrainingApi.Shared;

public static class IEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapTrainerApi(this IEndpointRouteBuilder app)
    {
        var clients = app.MapGroup("/clients/{id}")
    .AddEndpointFilterFactory((handlerContext, next) =>
    {
        var loggerFactory = handlerContext.ApplicationServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("RequestAuditor");
        return (invocationContext) =>
        {
            logger.LogInformation($"[⚙️] Received a request for: {invocationContext.HttpContext.Request.Path}");
            return next(invocationContext);
        };
    });

        clients.MapGet("", (int id, TrainingService service) => service.GetClientById(id));
        clients.MapPut("", (int id, Client updatedClient, TrainingService service)
            => service.UpdateClientById(id, updatedClient))
        .AddEndpointFilter(async (context, next) =>
        {
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

        return app;
    }
}