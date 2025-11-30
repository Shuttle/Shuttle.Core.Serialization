namespace Shuttle.Core.Serialization.Tests.v1;

public class SomeSerializerType
{
    public AnotherSerializerType AnotherSerializerType { get; set; } = new();
    public Guid Id { get; set; } = Guid.NewGuid();
}