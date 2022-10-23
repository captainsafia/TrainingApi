using Microsoft.EntityFrameworkCore;
using TrainingApi.Shared;

public static class DataGenerator
{
    public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
    {
        var serviceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
        var options = serviceProvider.GetRequiredService<DbContextOptions<TrainingDb>>();
        using var context = new TrainingDb(options);
        if (context.Clients.Any())
        {
            return app;
        }

        var clientA = new Client(1, "Vonnie", "Mawer", "vmawer0@go.com", 149, 66, DateTime.Parse("4/24/2000"));
        var clientB = new Client(2, "Langston", "Feldberg", "lfeldberg1@hc360.com", 329, 73, DateTime.Parse("10/20/1982"));
        var clientC = new Client(3, "Olwen", "Maeer", "omaeer3@purevolume.com", 261, 70, DateTime.Parse("8/22/1993"));

        context.Clients.AddRange(
            clientA,
            clientB,
            clientC
        );

        var trainerA = new Trainer(1, "Inna", "Spedroni", "ispedroni0@studiopress.com", Level.Junior, true);
        var trainerB = new Trainer(2, "Nikoletta", "Orrell", "norrell1@nydailynews.com", Level.Senior, true);
        var trainerC = new Trainer(3, "Briana", "Diprose", "bdiprose0@t.co", Level.Senior, true);
        var trainerD = new Trainer(4, "Zerk", "Riepl", "svanshin5@google.com", Level.Elite, true);

        context.Trainers.AddRange(
            trainerA,
            trainerB,
            trainerC,
            trainerD
        );

        context.SaveChanges();

        return app;
    }
}