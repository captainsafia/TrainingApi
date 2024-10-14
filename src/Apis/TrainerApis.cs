using TrainingApi.Services;
using TrainingApi.Shared;

namespace TrainingApi.Apis;

public static class TrainerApis
{
    public static IEndpointRouteBuilder MapTrainerApis(this IEndpointRouteBuilder app)
    {
        var trainers = app.MapGroup("/trainers")
            .RequireAuthorization("trainer_access");

        trainers.MapGet("/", (TrainersService service) => service.GetTrainers())
            .WithName("ListTrainers")
            .WithDescription("List all trainers");

        trainers.MapPut("/{id}", (int id, Trainer updatedTrainer, TrainersService service) =>
            service.UpdateTrainerById(id, updatedTrainer))
            .WithName("UpdateTrainer")
            .WithDescription("Update a trainer");

        trainers.MapDelete("/{id}", (int id, TrainersService service) => service.DeleteTrainerById(id))
            .WithName("DeleteTrainer")
            .WithDescription("Delete a trainer");

        trainers.MapPost("/", (TrainersService service, Trainer trainer) => service.CreateTrainer(trainer))
            .WithName("CreateTrainer")
            .WithDescription("Create a trainer");

        return app;
    }
}