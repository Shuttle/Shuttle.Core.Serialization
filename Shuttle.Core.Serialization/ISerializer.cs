using System;
using System.IO;

namespace Shuttle.Core.Serialization
{
    public interface ISerializer
    {
        string Name { get; }
        byte Id { get; }
        Stream Serialize(object instance);
        object Deserialize(Type type, Stream stream);
    }
}