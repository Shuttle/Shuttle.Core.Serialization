using System;

namespace Shuttle.Core.Serialization
{
    public interface ISerializerRootType
    {
        void AddSerializerType(Type root, Type contained);
    }
}