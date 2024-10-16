using System;
using Shuttle.Core.Serialization.Tests.v1;

namespace Shuttle.Core.Serialization.Tests;

public class ComplexSerializerType
{
    public AnotherSerializerType AnotherSerializerType1 { get; set; } = new();
    public v2.AnotherSerializerType AnotherSerializerType2 { get; set; } = new();

    public Guid Id { get; set; } = Guid.NewGuid();
    public SomeSerializerType SomeSerializerType1 { get; set; } = new();
    public v2.SomeSerializerType SomeSerializerType2 { get; set; } = new();
}