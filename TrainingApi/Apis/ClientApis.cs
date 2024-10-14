using TrainingApi.Shared;

namespace TrainingApi.Apis;

public static class ClientApis
{
    public static IEndpointRouteBuilder MapClientApis(this IEndpointRouteBuilder app)
    {
        var clients = app.MapGroup("/clients/{id}");
        clients.MapGet("", (int id, TrainingService service) => service.GetClientById(id));
        clients.MapPut("", (int id, Client updatedClient, TrainingService service)
            => service.UpdateClientById(id, updatedClient));

        return app;
    }
}