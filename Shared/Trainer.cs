namespace TrainingApi.Shared;

public record Trainer(int Id, string FirstName, string LastName, string Email, Level Level, bool IsCertificationActive)
{
    public Trainer()
        : this(0, "", "", "", Level.Junior, false)
    {
    }

    public int Id { get; set; } = Id; 

    public string FirstName { get; set; } = FirstName;
    public string LastName { get; set; } = LastName;
    public string Email { get; set; } = Email;

    public Level Level { get; set; } = Level;
    public bool IsCertificationActive { get; set; } = IsCertificationActive;
}
