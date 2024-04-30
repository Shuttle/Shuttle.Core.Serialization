using System.Collections.Generic;

namespace Shuttle.Core.Serialization
{
    public interface ISerializerService
    {
        ISerializerService Add(ISerializer serializer);
        ISerializer Get(string name);
        bool Contains(string name);
        IEnumerable<ISerializer> Serializers { get; }
    }
}