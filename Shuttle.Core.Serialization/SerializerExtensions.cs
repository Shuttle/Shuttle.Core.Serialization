using System.IO;
using System.Threading.Tasks;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization;

public static class SerializerExtensions
{
    public static async Task<T> DeserializeAsync<T>(this ISerializer serializer, Stream stream)
    {
        return (T)await Guard.AgainstNull(serializer).DeserializeAsync(typeof(T), Guard.AgainstNull(stream));
    }
}