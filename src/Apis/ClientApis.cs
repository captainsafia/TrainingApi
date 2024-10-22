using System.ComponentModel;
using TrainingApi.Services;
using TrainingApi.Shared;

namespace TrainingApi.Apis;

public static class ClientApis
{
    public static IEndpointRouteBuilder MapClientApis(this IEndpointRouteBuilder app)
    {
        var clients = app.MapGroup("/clients");

        clients.MapGet("/{id}", (int id, ClientsService service) =>
            service.GetClientById(id));

        clients.MapPut("/{id}", (int id, Client updatedClient, ClientsService service) =>
            service.UpdateClientById(id, updatedClient));

        clients.MapPost("", (ClientsService service, Client client) =>
            service.CreateClient(client));

        clients.MapDelete("/{id}", (int id, ClientsService service) =>
            service.DeleteClientById(id));

        return app;
    }
}