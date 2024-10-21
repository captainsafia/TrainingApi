using System.Text.Json.Serialization;

namespace TrainingApi.Shared;

[JsonConverter(typeof(JsonStringEnumConverter<Level>))]
public enum Level
{
    Junior,
    Senior,
    Elite
}