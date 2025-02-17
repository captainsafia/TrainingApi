using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TrainingApi.Shared;

/// <summary>
/// Represents a client with personal information.
/// </summary>
/// <param name="Id">The unique identifier of the client, assigned by the system when the client is created.</param>
/// <param name="FirstName">The first name of the client.</param>
/// <param name="LastName">The last name of the client.</param>
/// <param name="Email">The email of the client.</param>
/// <param name="Weight">The weight of the client in pounds, rounded to the nearest pound.</param>
/// <param name="Height">The height of the client in inches, rounded to the nearest inch.</param>
/// <param name="BirthDate">The date of birth of the client.</param>



public record Client(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    int Weight,
    int Height,
    DateTime BirthDate
) {
    /// <summary>The date of the last workout of the client.</summary>
    public DateOnly? LastWorkout { get; set; } = null;
    /// <summary>The training goal of the client.</summary>
    public TrainingGoal? Goal { get; set; } = null;
    /// <summary>Notes about the client.</summary>
    public string? Notes { get; set; } = null;
};

/// <summary>
/// The training goals of the client.
/// </summary>
/// <param name="FatLoss">Fat Loss</param>
/// <param name="MuscleGain">Muscle Gain</param>
/// <param name="Endurance">Endurance</param>
/// <param name="Flexibility">Flexibility</param>
/// <param name="Strength">Strength</param>
/// <param name="GeneralHealth">General Health</param>
///
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TrainingGoal
{
    FatLoss,
    MuscleGain,
    Endurance,
    Flexibility,
    Strength,
    GeneralHealth
}
