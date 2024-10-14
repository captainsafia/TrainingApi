using TrainingApi.Services;
using TrainingApi.Shared;

namespace TrainingApi.Apis;

public static class TrainerApis
{
    public static IEndpointRouteBuilder MapTrainerApis(this IEndpointRouteBuilder app)
    {
        var trainers = app.MapGroup("/trainers")
            .RequireAuthorization("trainer_access");

        trainers.MapGet("/", (TrainersService service) => service.GetTrainers());
        trainers.MapPut("/{id}", (int id, Trainer updatedTrainer, TrainersService service) =>
            service.UpdateTrainerById(id, updatedTrainer));
        trainers.MapDelete("/{id}", (int id, TrainersService service) => service.DeleteTrainerById(id));
        trainers.MapPost("/", (TrainersService service, Trainer trainer) => service.CreateTrainer(trainer));

        return app;
    }   
}