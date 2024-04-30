using System;
using System.Collections.Generic;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization
{
    public class SerializerService : ISerializerService
    {
        private readonly Dictionary<string, ISerializer> _serializers = new Dictionary<string, ISerializer>();
        
        public ISerializerService Add(ISerializer serializer)
        {
            Guard.AgainstNull(serializer, nameof(serializer));

            if (_serializers.ContainsKey(serializer.Name))
            {
                throw new ArgumentException(string.Format(Resources.DuplicateSerializerException, serializer.Name));
            }

            _serializers.Add(serializer.Name, serializer);

            return this;
        }

        public ISerializer Get(string name)
        {
            Guard.AgainstNullOrEmptyString(name, nameof(name));

            if (!_serializers.ContainsKey(name))
            {
                throw new ArgumentException(string.Format(Resources.SerializerMissingException, name));
            }

            return _serializers[name];
        }

        public bool Contains(string name)
        {
            return _serializers.ContainsKey(Guard.AgainstNullOrEmptyString(name, nameof(name)));
        }

        public IEnumerable<ISerializer> Serializers => _serializers.Values;
    }
}