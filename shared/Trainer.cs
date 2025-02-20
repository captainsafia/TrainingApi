using System.ComponentModel;

namespace TrainingApi.Shared;

public record Trainer(int Id, string FirstName, string LastName, string Email, Level Level, bool IsCertificationActive)
{
    public Trainer()
        : this(0, "", "", "", Level.Junior, false)
    {
    }

    /// <summary>
    /// The unique identifier of the trainer, assigned by the system when the trainer is created.
    /// </summary>
    public int Id { get; set; } = Id;

    /// <summary>
    /// The first name of the trainer.
    /// </summary>
    public string FirstName { get; set; } = FirstName;

    /// <summary>
    /// The last name of the trainer.
    /// </summary>
    public string LastName { get; set; } = LastName;

    /// <summary>
    /// The email address of the trainer.
    /// </summary>
    public string Email { get; set; } = Email;

    /// <summary>
    /// The level of the trainer.
    /// </summary>
    public Level Level { get; set; } = Level;

    /// <summary>
    /// Indicates whether the trainer's certification is active.
    /// </summary>
    public bool IsCertificationActive { get; set; } = IsCertificationActive;
}
