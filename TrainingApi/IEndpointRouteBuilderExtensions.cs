using TrainingApi.Shared;

public static class IEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapTrainerApi(this IEndpointRouteBuilder app)
    {
        var clients = app.MapGroup("/clients/{id}");

        clients.MapGet("", (int id, TrainingService service) => service.GetClientById(id));
        clients.MapPut("", (int id, Client updatedClient, TrainingService service)
            => service.UpdateClientById(id, updatedClient))
            .AddEndpointFilter(async (context, next) =>
            {
                var client = context.GetArgument<Client>(2);
                Dictionary<string, string[]> errors = new();
                if (client.FirstName.Any(char.IsDigit))
                {
                    errors.Add("firstName", ["First names cannot contain any numeric characters."]);
                }
                if (client.LastName.Any(char.IsDigit))
                {
                    errors.Add("lastName", ["Last names cannot contain any numeric characters."]);
                }
                if (errors.Count() > 0)
                {
                    return TypedResults.ValidationProblem(errors);
                }
                return await next(context);
            });

        var trainers = app.MapGroup("/trainers")
            .RequireAuthorization("trainer_access");

        trainers.MapGet("/", (TrainingService service, PagingData pagingData)
            => service.GetTrainers(pagingData.ItemCount, pagingData.CurrentPage));
        trainers.MapPut("/{id}", (int id, Trainer updatedTrainer, TrainingService service) =>
            service.UpdateTrainerById(id, updatedTrainer));
        trainers.MapDelete("/{id}", (int id, TrainingService service) => service.DeleteTrainerById(id));
        trainers.MapPost("/", (TrainingService service, Trainer trainer) => service.CreateTrainer(trainer));

        return app;
    }
}