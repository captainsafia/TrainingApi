using System;
using System.ComponentModel;

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
);
