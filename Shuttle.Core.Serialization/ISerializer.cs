namespace Shuttle.Core.Serialization;

public interface ISerializer
{
    string Name { get; }
    Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default);
    Task<Stream> SerializeAsync(object instance, CancellationToken cancellationToken = default);
}