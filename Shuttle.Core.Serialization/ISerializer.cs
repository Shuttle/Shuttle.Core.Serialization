using System;
using System.IO;
using System.Threading.Tasks;

namespace Shuttle.Core.Serialization;

public interface ISerializer
{
    string Name { get; }
    Task<object> DeserializeAsync(Type type, Stream stream);
    Task<Stream> SerializeAsync(object instance);
}