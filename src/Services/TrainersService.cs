using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingApi.Shared;

namespace TrainingApi.Services;

public class TrainersService(TrainingDb trainingDb)
{
    public async Task<Results<Created<Trainer>, NotFound<ProblemDetails>>> UpdateTrainerById(int id, Trainer updatedTrainer)
    {
        var trainer = await trainingDb.Trainers.FindAsync(id);
        if (trainer is null) return TypedResults.NotFound(new ProblemDetails());
        trainer = updatedTrainer;
        await trainingDb.SaveChangesAsync();
        return TypedResults.Created($"/trainers/{trainer.Id}", trainer);
    }

    public Results<Created<Trainer>, NotFound<ProblemDetails>> CreateTrainer(Trainer trainer)
    {
        trainingDb.Trainers.Add(trainer);
        return TypedResults.Created($"/trainers/{trainer.Id}", trainer);
    }

    public async Task<Results<Ok<List<Trainer>>, NotFound<ProblemDetails>>> GetTrainers()
    {
        var trainers = await trainingDb.Trainers.ToListAsync();
        return TypedResults.Ok(trainers);
    }

    public async Task<Results<NoContent, NotFound<ProblemDetails>>> DeleteTrainerById(int id)
    {
        var trainer = await trainingDb.Trainers.FindAsync(id);
        if (trainer is null) return TypedResults.NotFound(new ProblemDetails());
        trainingDb.Trainers.Remove(trainer);
        return TypedResults.NoContent();
    }

    public async Task<Results<Ok<Trainer>, NotFound<ProblemDetails>>> GetTrainerById(int id)
    {
        var trainer = await trainingDb.Trainers.FindAsync(id);
        return trainer is null ? TypedResults.NotFound(new ProblemDetails()) : TypedResults.Ok(trainer);
    }
}