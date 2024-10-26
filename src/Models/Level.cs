using System.Text.Json.Serialization;

namespace TrainingApi.Models;

[JsonConverter(typeof(JsonStringEnumConverter<Level>))]
public enum Level
{
    Junior,
    Senior,
    Elite
}