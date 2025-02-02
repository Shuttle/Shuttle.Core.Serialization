using System;

namespace Shuttle.Core.Serialization.Tests.v1
{
    public class SomeSerializerType
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public AnotherSerializerType AnotherSerializerType { get; set; } = new();
    }
}