using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TrainingApi.Shared;

namespace TrainingApi.Services;

public class TrainersService(TrainingDb trainingDb)
{
    public async Task<Results<Created<Trainer>, NotFound>> UpdateTrainerById(int id, Trainer updatedTrainer)
    {
        var trainer = await trainingDb.Trainers.FindAsync(id);
        if (trainer is null) return TypedResults.NotFound();
        trainer = updatedTrainer;
        await trainingDb.SaveChangesAsync();
        return TypedResults.Created($"/trainers/{trainer.Id}", trainer);
    }

    public Results<Created<Trainer>, NotFound> CreateTrainer(Trainer trainer)
    {
        trainingDb.Trainers.Add(trainer);
        return TypedResults.Created($"/trainers/{trainer.Id}", trainer);
    }

    public async Task<Results<Ok<List<Trainer>>, NotFound>> GetTrainers()
    {
        var trainers = await trainingDb.Trainers.ToListAsync();
        return TypedResults.Ok(trainers);
    }

    public async Task<Results<NoContent, NotFound>> DeleteTrainerById(int id)
    {
        var trainer = await trainingDb.Trainers.FindAsync(id);
        if (trainer is null) return TypedResults.NotFound();
        trainingDb.Trainers.Remove(trainer);
        return TypedResults.NoContent();
    }

    public async Task<Results<Ok<Trainer>, NotFound>> GetTrainerById(int id)
    {
        var trainer = await trainingDb.Trainers.FindAsync(id);
        return trainer is null ? TypedResults.NotFound() : TypedResults.Ok(trainer);
    }
}