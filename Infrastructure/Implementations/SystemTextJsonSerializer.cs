using System.Text.Json;
using System.Text.Json.Serialization;
using Grimoire.Infrastructure.Abstractions;

namespace Grimoire.Infrastructure.Implementations;

/// <summary>
/// Default JSON serializer that leverages <see cref="System.Text.Json"/>.
/// </summary>
public sealed class SystemTextJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _options;

    public SystemTextJsonSerializer()
    {
        _options = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };
        _options.Converters.Add(new JsonStringEnumConverter());
    }

    public string Serialize(object obj) => JsonSerializer.Serialize(obj, _options);

    public T Deserialize<T>(string json)
        => JsonSerializer.Deserialize<T>(json, _options)!;
} 