using System.ComponentModel;

namespace TrainingApi.Shared;

public record Trainer(int Id, string FirstName, string LastName, string Email, Level Level, bool IsCertificationActive)
{
    public Trainer()
        : this(0, "", "", "", Level.Junior, false)
    {
    }

    [Description("The unique identifier of the trainer, assigned by the system when the trainer is created")]
    public int Id { get; set; } = Id;

    [Description("The first name of the trainer")]
    public string FirstName { get; set; } = FirstName;
    [Description("The last name of the trainer")]
    public string LastName { get; set; } = LastName;
    [Description("The email address of the trainer")]
    public string Email { get; set; } = Email;
    [Description("The level of the trainer")]
    public Level Level { get; set; } = Level;
    public bool IsCertificationActive { get; set; } = IsCertificationActive;
}
