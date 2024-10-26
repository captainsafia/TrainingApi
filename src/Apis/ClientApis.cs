using System.ComponentModel;
using Microsoft.AspNetCore.Http.HttpResults;
using TrainingApi.Services;
using TrainingApi.Models;

namespace TrainingApi.Apis;

public static class ClientApis
{
    public static IEndpointRouteBuilder MapClientApis(this IEndpointRouteBuilder app)
    {
        var clients = app.MapGroup("/clients")
            .WithTags("Clients");

        clients.MapGet("/{id}", GetClientById);

        clients.MapPut("/{id}", (
             [Description("The unique identifier of the client, assigned by the system when the client is created")] int id,
            Client updatedClient,
            ClientsService service) => service.UpdateClientById(id, updatedClient))
            .WithName("UpdateClient")
            .WithDescription("Update a client");

        clients.MapPost("", (ClientsService service, Client client) => service.CreateClient(client))
            .WithName("CreateClient")
            .WithDescription("Create a client");

        clients.MapPost("/{id}", (
             [Description("The unique identifier of the client, assigned by the system when the client is created")] int id,
             ClientsService service) => service.DeleteClientById(id))
            .WithName("DeleteClient")
            .WithDescription("Delete a client");

        return app;
    }

    /// <summary>
    /// Get a client.
    /// </summary>
    /// <param name="id">The unique identifier of the client, assigned by the system when the client is created</param>
    public static Task<Results<Ok<Client>, NotFound>> GetClientById(int id, ClientsService service)
    {
        return service.GetClientById(id);
    }
}