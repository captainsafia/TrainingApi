using TrainingApi.Services;
using TrainingApi.Shared;

namespace TrainingApi.Apis;

public static class ClientApis
{
    public static IEndpointRouteBuilder MapClientApis(this IEndpointRouteBuilder app)
    {
        var clients = app.MapGroup("/clients");
        clients.MapGet("/{id}", (int id, ClientsService service) => service.GetClientById(id))
            .WithName("GetClient")
            .WithDescription("Get a client");

        clients.MapPut("/{id}", (int id, Client updatedClient, ClientsService service)
            => service.UpdateClientById(id, updatedClient))
            .WithName("UpdateClient")
            .WithDescription("Update a client");

        clients.MapPost("", (ClientsService service, Client client) => service.CreateClient(client))
            .WithName("CreateClient")
            .WithDescription("Create a client");

        clients.MapPost("/{id}", (int id, ClientsService service) => service.DeleteClientById(id))
            .WithName("DeleteClient")
            .WithDescription("Delete a client");

        return app;
    }
}