using Shuttle.Contract;

namespace Shuttle.Serialization;

public static class SerializerExtensions
{
    extension(ISerializer serializer)
    {
        public async Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            return (T)await Guard.AgainstNull(serializer).DeserializeAsync(typeof(T), Guard.AgainstNull(stream), cancellationToken);
        }
    }
}