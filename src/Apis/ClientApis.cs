using System.ComponentModel;
using TrainingApi.Services;
using TrainingApi.Shared;

namespace TrainingApi.Apis;

public static class ClientApis
{
    public static IEndpointRouteBuilder MapClientApis(this IEndpointRouteBuilder app)
    {
        var clients = app.MapGroup("/clients")
            .WithTags("Clients");

        clients.MapGet("/{id}", (
            [Description("The unique identifier of the client, assigned by the system when the client is created")] int id,
            ClientsService service) => service.GetClientById(id))
            .WithName("GetClient")
            .WithSummary("Get a client")
            .WithDescription("Get the client with the specified id.");

        clients.MapPut("/{id}", (
             [Description("The unique identifier of the client, assigned by the system when the client is created")] int id,
            Client updatedClient,
            ClientsService service) => service.UpdateClientById(id, updatedClient))
            .WithName("UpdateClient")
            .WithSummary("Update a client")
            .WithDescription("Update the client with the specified id using the information passed in the request body.");

        clients.MapPost("", (ClientsService service, Client client) => service.CreateClient(client))
            .WithName("CreateClient")
            .WithSummary("Create a client")
            .WithDescription("Create a new client using the information passed in the request body.");

        clients.MapDelete("/{id}", (
             [Description("The unique identifier of the client, assigned by the system when the client is created")] int id,
             ClientsService service) => service.DeleteClientById(id))
            .WithName("DeleteClient")
            .WithSummary("Delete a client")
            .WithDescription("Delete the client with the specified id.");

        return app;
    }
}