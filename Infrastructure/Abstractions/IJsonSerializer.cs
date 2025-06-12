namespace Grimoire.Infrastructure.Abstractions;

public interface IJsonSerializer
{
    string Serialize(object obj);
    T Deserialize<T>(string json);
}