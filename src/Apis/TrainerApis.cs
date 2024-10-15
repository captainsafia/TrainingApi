using System.ComponentModel;
using TrainingApi.Services;
using TrainingApi.Shared;

namespace TrainingApi.Apis;

public static class TrainerApis
{
    public static IEndpointRouteBuilder MapTrainerApis(this IEndpointRouteBuilder app)
    {
        var trainers = app.MapGroup("/trainers")
            .RequireAuthorization("trainer_access")
            .WithTags("Trainers");

        trainers.MapGet("/", (TrainersService service) => service.GetTrainers())
            .WithName("ListTrainers")
            .WithDescription("List all trainers");

        trainers.MapPut("/{id}", (
            [Description("The unique identifier of the trainer, assigned by the system when the client is created")] int id,
            Trainer updatedTrainer,
            TrainersService service) => service.UpdateTrainerById(id, updatedTrainer))
            .WithName("UpdateTrainer")
            .WithDescription("Update a trainer");

        trainers.MapDelete("/{id}", (
            [Description("The unique identifier of the trainer, assigned by the system when the client is created")] int id,
            TrainersService service) => service.DeleteTrainerById(id))
            .WithName("DeleteTrainer")
            .WithDescription("Delete a trainer");

        trainers.MapPost("/", (TrainersService service, Trainer trainer) => service.CreateTrainer(trainer))
            .WithName("CreateTrainer")
            .WithDescription("Create a trainer");

        return app;
    }
}