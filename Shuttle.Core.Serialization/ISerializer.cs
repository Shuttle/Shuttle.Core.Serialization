using System;
using System.IO;
using System.Threading.Tasks;

namespace Shuttle.Core.Serialization
{
    public interface ISerializer
    {
        string Name { get; }
        Stream Serialize(object instance);
        object Deserialize(Type type, Stream stream);
        Task<Stream> SerializeAsync(object instance);
        Task<object> DeserializeAsync(Type type, Stream stream);
    }
}