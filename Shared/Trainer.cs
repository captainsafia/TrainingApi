namespace TrainingApi.Shared;

public record Trainer(int Id, string FirstName, string LastName, string Email, Level Level, bool IsCertificationActive);