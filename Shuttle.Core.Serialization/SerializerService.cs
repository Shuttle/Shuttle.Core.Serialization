using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization;

public class SerializerService : ISerializerService
{
    private readonly Dictionary<string, ISerializer> _serializers = new();

    public ISerializerService Add(ISerializer serializer)
    {
        Guard.AgainstNull(serializer);

        return !_serializers.TryAdd(serializer.Name, serializer)
            ? throw new ArgumentException(string.Format(Resources.DuplicateSerializerException, serializer.Name))
            : this;
    }

    public ISerializer Get(string name)
    {
        Guard.AgainstEmpty(name);

        return !_serializers.TryGetValue(name, out var serializer)
            ? throw new ArgumentException(string.Format(Resources.SerializerMissingException, name))
            : serializer;
    }

    public bool Contains(string name)
    {
        return _serializers.ContainsKey(Guard.AgainstEmpty(name));
    }

    public IEnumerable<ISerializer> Serializers => _serializers.Values;
}