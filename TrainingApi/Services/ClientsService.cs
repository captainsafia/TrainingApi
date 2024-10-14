using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TrainingApi.Shared;

namespace TrainingApi.Services;

public class ClientsService(TrainingDb trainingDb)
{
        public async Task<Results<Ok<Client>, NotFound>> GetClientById(int id)
    {
        var client = await trainingDb.Clients.FindAsync(id);
        return client is null ? TypedResults.NotFound() : TypedResults.Ok(client);
    }
    public async Task<Results<Created<Client>, NotFound>> UpdateClientById(int id, Client updatedClient)
    {
        var client = await trainingDb.Clients.FindAsync(id);
        if (client is null) return TypedResults.NotFound();
        client = updatedClient;
        await trainingDb.SaveChangesAsync();
        return TypedResults.Created($"/clients/{client.Id}", client);
    }

    public async Task<Results<Ok<Client>, NotFound>> DeleteClientById(int id)
    {
        var client = await trainingDb.Clients.FindAsync(id);
        if (client is null) return TypedResults.NotFound();
        trainingDb.Clients.Remove(client);
        return TypedResults.Ok(client);
    }

    public Results<Created<Client>, NotFound> CreateClient(Client client)
    {
        trainingDb.Clients.Add(client);
        return TypedResults.Created($"/clients/{client.Id}", client);
    }

    public async Task<Results<Ok<List<Client>>, NotFound>> GetClients()
    {
        var clients = await trainingDb.Clients.ToListAsync();
        return TypedResults.Ok(clients);
    }
}