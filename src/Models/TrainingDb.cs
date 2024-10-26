using Microsoft.EntityFrameworkCore;

namespace TrainingApi.Models;

public class TrainingDb : DbContext
{
    // Parameterless constructor to support mocking in unit tests
    public TrainingDb() { }
    public TrainingDb(DbContextOptions options) : base(options) { }

    // Virtual DbSets to support mocking in unit tests
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Trainer> Trainers { get; set; }
}