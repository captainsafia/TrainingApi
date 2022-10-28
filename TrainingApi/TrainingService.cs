using TrainingApi.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class TrainingService
{
    private readonly TrainingDb _db;
    
    public TrainingService(TrainingDb db)
    {
        _db = db;
    }
    public async Task<Results<Ok<Client>, NotFound>> GetClientById(int id)
    {
        var client = await _db.Clients.FindAsync(id);
        return client is null ? TypedResults.NotFound() : TypedResults.Ok(client);
    }
    public async Task<Results<Created<Client>, NotFound>> UpdateClientById(int id, Client updatedClient)
    {
        var client = await _db.Clients.FindAsync(id);
        if (client is null) return TypedResults.NotFound();
        client = updatedClient;
        await _db.SaveChangesAsync();
        return TypedResults.Created($"/clients/{client.Id}", client);
    }

    public async Task<Results<Ok<Client>, NotFound>> DeleteClientById(int id)
    {
        var client = await _db.Clients.FindAsync(id);
        if (client is null) return TypedResults.NotFound();
        _db.Clients.Remove(client);
        return TypedResults.Ok(client);
    }

    public Results<Created<Client>, NotFound> CreateClient(Client client)
    {
        _db.Clients.Add(client);
        return TypedResults.Created($"/clients/{client.Id}", client);
    }

    public async Task<Results<Ok<List<Client>>, NotFound>> GetClients()
    {
        var clients = await _db.Clients.ToListAsync();
        return TypedResults.Ok(clients);
    }

    public async Task<Results<Created<Trainer>, NotFound>> UpdateTrainerById(int id, Trainer updatedTrainer)
    {
        var trainer = await _db.Trainers.FindAsync(id);
        if (trainer is null) return TypedResults.NotFound();
        trainer = updatedTrainer;
        await _db.SaveChangesAsync();
        return TypedResults.Created($"/trainers/{trainer.Id}", trainer);
    }

    public Results<Created<Trainer>, NotFound> CreateTrainer(Trainer trainer)
    {
        _db.Trainers.Add(trainer);
        return TypedResults.Created($"/trainers/{trainer.Id}", trainer);
    }

    public async Task<Results<Ok<List<Trainer>>, NotFound>> GetTrainers()
    {
        var trainers = await _db.Trainers.ToListAsync();
        return TypedResults.Ok(trainers);
    }

    public async Task<Results<Ok<Trainer>, NotFound>> DeleteTrainerById(int id)
    {
        var trainer = await _db.Trainers.FindAsync(id);
        if (trainer is null) return TypedResults.NotFound();
        _db.Trainers.Remove(trainer);
        return TypedResults.Ok(trainer);
    }

    public async Task<Results<Ok<Trainer>, NotFound>> GetTrainerById(int id)
    {
        var trainer = await _db.Trainers.FindAsync(id);
        return trainer is null ? TypedResults.NotFound() : TypedResults.Ok(trainer);
    }
}