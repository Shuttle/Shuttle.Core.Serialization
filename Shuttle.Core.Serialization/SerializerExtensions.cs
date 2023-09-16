using System.IO;
using System.Threading.Tasks;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization
{
    public static class SerializerExtensions
    {
        public static T Deserialize<T>(this ISerializer serializer, Stream stream)
        {
            return (T)Guard.AgainstNull(serializer, nameof(serializer)).Deserialize(typeof(T), Guard.AgainstNull(stream, nameof(stream)));
        }

        public static async Task<T> DeserializeAsync<T>(this ISerializer serializer, Stream stream)
        {
            return (T)(await Guard.AgainstNull(serializer, nameof(serializer)).DeserializeAsync(typeof(T), Guard.AgainstNull(stream, nameof(stream))));
        }
    }
}