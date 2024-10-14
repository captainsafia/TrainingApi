using TrainingApi.Shared;

namespace TrainingApi.Apis;

public static class TrainerApis
{
    public static IEndpointRouteBuilder MapTrainerApis(this IEndpointRouteBuilder app)
    {
        var trainers = app.MapGroup("/trainers")
            .RequireAuthorization("trainer_access");

        trainers.MapGet("/", (TrainingService service) => service.GetTrainers());
        trainers.MapPut("/{id}", (int id, Trainer updatedTrainer, TrainingService service) =>
            service.UpdateTrainerById(id, updatedTrainer));
        trainers.MapDelete("/{id}", (int id, TrainingService service) => service.DeleteTrainerById(id));
        trainers.MapPost("/", (TrainingService service, Trainer trainer) => service.CreateTrainer(trainer));

        return app;
    }   
}