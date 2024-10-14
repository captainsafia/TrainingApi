using System.ComponentModel;

namespace TrainingApi.Shared;

public record Client(
    [property: Description("The unique identifier of the client, assigned by the system when the client is created")]
    int Id,
    [property: Description("The first name of the client")]
    string FirstName,
    [property: Description("The last name of the client")]
    string LastName,
    [property: Description("The email of the client")]
    string Email,
    [property: Description("The weight of the client in pounds, rounded to the nearest pound.")]
    int Weight,
    [property: Description("The height of the client in inches, rounded to the nearest inch.")]
    int Height,
    [property: Description("The date of birth of the client")]
    DateTime BirthDate
);
