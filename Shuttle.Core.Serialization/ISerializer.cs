using System;
using System.IO;
using System.Threading.Tasks;

namespace Shuttle.Core.Serialization
{
    public interface ISerializer
    {
        string Name { get; }
        Task<Stream> Serialize(object instance);
        Task<object> Deserialize(Type type, Stream stream);
    }
}