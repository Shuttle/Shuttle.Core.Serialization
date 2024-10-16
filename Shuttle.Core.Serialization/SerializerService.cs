using System;
using System.Collections.Generic;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization;

public class SerializerService : ISerializerService
{
    private readonly Dictionary<string, ISerializer> _serializers = new();

    public ISerializerService Add(ISerializer serializer)
    {
        Guard.AgainstNull(serializer);

        if (!_serializers.TryAdd(serializer.Name, serializer))
        {
            throw new ArgumentException(string.Format(Resources.DuplicateSerializerException, serializer.Name));
        }

        return this;
    }

    public ISerializer Get(string name)
    {
        Guard.AgainstNullOrEmptyString(name);

        if (!_serializers.TryGetValue(name, out var serializer))
        {
            throw new ArgumentException(string.Format(Resources.SerializerMissingException, name));
        }

        return serializer;
    }

    public bool Contains(string name)
    {
        return _serializers.ContainsKey(Guard.AgainstNullOrEmptyString(name));
    }

    public IEnumerable<ISerializer> Serializers => _serializers.Values;
}