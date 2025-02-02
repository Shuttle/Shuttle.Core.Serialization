using System.Collections.Generic;

namespace Shuttle.Core.Serialization;

public interface ISerializerService
{
    IEnumerable<ISerializer> Serializers { get; }
    ISerializerService Add(ISerializer serializer);
    bool Contains(string name);
    ISerializer Get(string name);
}