namespace Shuttle.Serialization.Tests.v2;

public class SomeSerializerType
{
    public AnotherSerializerType AnotherSerializerType { get; set; } = new();
    public Guid Id { get; set; } = Guid.NewGuid();
}