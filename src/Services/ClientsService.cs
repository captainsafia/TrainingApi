using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingApi.Shared;

namespace TrainingApi.Services;

public class ClientsService(TrainingDb trainingDb)
{
        public async Task<Results<Ok<Client>, NotFound<ProblemDetails>>> GetClientById(int id)
    {
        var client = await trainingDb.Clients.FindAsync(id);
        return client is null ? TypedResults.NotFound(new ProblemDetails()) : TypedResults.Ok(client);
    }
    public async Task<Results<Created<Client>, NotFound<ProblemDetails>>> UpdateClientById(int id, Client updatedClient)
    {
        var client = await trainingDb.Clients.FindAsync(id);
        if (client is null) return TypedResults.NotFound(new ProblemDetails{});
        client = updatedClient;
        await trainingDb.SaveChangesAsync();
        return TypedResults.Created($"/clients/{client.Id}", client);
    }

    public async Task<Results<NoContent, NotFound<ProblemDetails>>> DeleteClientById(int id)
    {
        var client = await trainingDb.Clients.FindAsync(id);
        if (client is null) return TypedResults.NotFound(new ProblemDetails());
        trainingDb.Clients.Remove(client);
        return TypedResults.NoContent();
    }

    public Results<Created<Client>, NotFound<ProblemDetails>> CreateClient(Client client)
    {
        trainingDb.Clients.Add(client);
        return TypedResults.Created($"/clients/{client.Id}", client);
    }

    public async Task<Results<Ok<List<Client>>, NotFound<ProblemDetails>>> GetClients()
    {
        var clients = await trainingDb.Clients.ToListAsync();
        return TypedResults.Ok(clients);
    }
}