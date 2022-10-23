using Microsoft.EntityFrameworkCore;

namespace TrainingApi.Shared;

public class TrainingDb : DbContext
{
    public TrainingDb(DbContextOptions options) : base(options) { }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
}