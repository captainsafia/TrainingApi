using Microsoft.EntityFrameworkCore;

class TrainingDb : DbContext
{
    public TrainingDb(DbContextOptions options) : base(options) { }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
}

public class Client
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int Weight { get; set; }
    public DateTime BirthDate { get; set; }
}

public class Trainer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public Level Level { get; set; }

    public bool IsCertificationActive { get; set; }
}

public enum Level
{
    Junior,
    Senior,
    Elite
}

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

        var clientA = new Client()
        {
            Id = 1,
            FirstName = "Vonnie",
            LastName = "Mawer",
            Email = "vmawer0@go.com",
            Weight = 149,
            BirthDate = DateTime.Parse("4/24/2000")
        };

        var clientB = new Client()
        {
            Id = 2,
            FirstName = "Langston",
            LastName = "Feldberg",
            Email = "lfeldberg1@hc360.com",
            Weight = 329,
            BirthDate = DateTime.Parse("10/20/1982")
        };
        var clientC = new Client()
        {
            Id = 3,
            FirstName = "Olwen",
            LastName = "Maeer",
            Email = "omaeer3@purevolume.com",
            Weight = 261,
            BirthDate = DateTime.Parse("8/22/1993")
        }; 

        context.Clients.AddRange(
            clientA,
            clientB,
            clientC
        );

        var trainerA = new Trainer()
        {
            Id = 1,
            FirstName = "Inna",
            LastName = "Spedroni",
            Email = "ispedroni0@studiopress.com",
            Level = Level.Junior,
            IsCertificationActive = true
        };

        var trainerB = new Trainer()
        {
            Id = 2,
            FirstName = "Nikoletta",
            LastName = "Orrell",
            Email = "norrell1@nydailynews.com",
            Level = Level.Senior,
            IsCertificationActive = true
        };

        var trainerC = new Trainer()
        {
            Id = 3,
            FirstName = "Briana",
            LastName = "Diprose",
            Email = "bdiprose0@t.co",
            Level = Level.Senior,
            IsCertificationActive = true
        };

        var trainerD = new Trainer()
        {
            Id = 4,
            FirstName = "Zerk",
            LastName = "Riepl",
            Email = "svanshin5@google.com",
            Level = Level.Elite,
            IsCertificationActive = true
        };

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