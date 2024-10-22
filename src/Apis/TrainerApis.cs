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
            .WithSummary("List all trainers")
            .WithDescription("Get a list of all trainers.");

        trainers.MapPut("/{id}", (
            [Description("The unique identifier of the trainer, assigned by the system when the client is created")] int id,
            Trainer updatedTrainer,
            TrainersService service) => service.UpdateTrainerById(id, updatedTrainer))
            .WithName("UpdateTrainer")
            .WithSummary("Update a trainer")
            .WithDescription("Update the trainer with the specified id using the information passed in the request body.");

        trainers.MapDelete("/{id}", (
            [Description("The unique identifier of the trainer, assigned by the system when the client is created")] int id,
            TrainersService service) => service.DeleteTrainerById(id))
            .WithName("DeleteTrainer")
            .WithSummary("Delete a trainer")
            .WithDescription("Delete the trainer with the specified id.");

        trainers.MapPost("/", (TrainersService service, Trainer trainer) => service.CreateTrainer(trainer))
            .WithName("CreateTrainer")
            .WithSummary("Create a trainer")
            .WithDescription("Create a new trainer using the information passed in the request body.");

        return app;
    }
}