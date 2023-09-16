using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization
{
    public class DefaultSerializer : ISerializer, ISerializerRootType
    {
        private static readonly object Padlock = new object();
        private readonly XmlSerializerNamespaces _namespaces = new XmlSerializerNamespaces();

        private readonly Dictionary<Type, XmlAttributeOverrides> _overrides =
            new Dictionary<Type, XmlAttributeOverrides>();

        private readonly Dictionary<Type, XmlSerializer> _serializers = new Dictionary<Type, XmlSerializer>();

        private readonly XmlWriterSettings _xmlWriterSettings;
        private readonly XmlDictionaryReaderQuotas _xmlDictionaryReaderQuotas;

        public DefaultSerializer()
        {
            _xmlWriterSettings = new XmlWriterSettings
            {
                Async = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true,
                Indent = true
            };

            _xmlDictionaryReaderQuotas = new XmlDictionaryReaderQuotas
            {
                MaxArrayLength = int.MaxValue,
                MaxStringContentLength = int.MaxValue,
                MaxNameTableCharCount = int.MaxValue
            };

            _namespaces.Add(string.Empty, string.Empty);
        }

        public string Name => "Xml";

        public Stream Serialize(object instance)
        {
            return SerializeAsync(instance, true).GetAwaiter().GetResult();
        }

        public async Task<Stream> SerializeAsync(object instance)
        {
            return await SerializeAsync(instance, false).ConfigureAwait(false);
        }

        private async Task<Stream> SerializeAsync(object instance, bool sync)
        {
            Guard.AgainstNull(instance, nameof(instance));

            var messageType = instance.GetType();
            var serializer = GetSerializer(messageType);

            var xml = new StringBuilder();

            using var writer = XmlWriter.Create(xml, _xmlWriterSettings);
            
            serializer.Serialize(writer, instance, _namespaces);

            if (sync)
            {
                writer.Flush();
            }
            else
            {
                await writer.FlushAsync().ConfigureAwait(false);
            }

            var data = Encoding.UTF8.GetBytes(xml.ToString());
            
            return new MemoryStream(data, 0, data.Length, false, true);
        }

        public object Deserialize(Type type, Stream stream)
        {
            return DeserializeAsync(type, stream, true).GetAwaiter().GetResult();
        }

        public async Task<object> DeserializeAsync(Type type, Stream stream)
        {
            return await DeserializeAsync(type, stream, false).ConfigureAwait(false);
        }

        private async Task<object> DeserializeAsync(Type type, Stream stream, bool sync)
        {
            Guard.AgainstNull(type, nameof(type));
            Guard.AgainstNull(stream, nameof(stream));

            using var copy = new MemoryStream();
            var position = stream.Position;

            stream.Position = 0;

            if (sync)
            {
                stream.CopyTo(copy);
            }
            else
            {
                await stream.CopyToAsync(copy).ConfigureAwait(false);
            }

            stream.Position = position;
            copy.Position = 0;

            using var reader = XmlDictionaryReader.CreateTextReader(copy, Encoding.UTF8, _xmlDictionaryReaderQuotas, null);

            return GetSerializer(type).Deserialize(reader);
        }

        public void AddSerializerType(Type root, Type contained)
        {
            Guard.AgainstNull(root, nameof(root));
            Guard.AgainstNull(contained, nameof(contained));

            if (HasSerializerType(root, contained))
            {
                return;
            }

            lock (Padlock)
            {
                if (HasSerializerType(root, contained))
                {
                    return;
                }

                if (!_overrides.TryGetValue(root, out var overrides))
                {
                    overrides = new XmlAttributeOverrides();
                    _overrides.Add(root, overrides);
                }

                overrides.Add(contained,
                    new XmlAttributes {XmlRoot = new XmlRootAttribute {Namespace = contained.Namespace}});

                foreach (var nested in root.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!HasSerializerType(root, nested))
                    {
                        AddSerializerType(root, nested);
                    }
                }

                _serializers.Clear();
            }
        }

        private bool HasSerializerType(Type root, Type contained)
        {
            lock (Padlock)
            {
                if (!_overrides.TryGetValue(root, out var overrides))
                {
                    return false;
                }

                return overrides[contained] != null;
            }
        }

        private XmlSerializer GetSerializer(Type type)
        {
            lock (Padlock)
            {
                if (!_overrides.TryGetValue(type, out var overrides))
                {
                    overrides = new XmlAttributeOverrides();
                    _overrides.Add(type, overrides);
                }

                if (!_serializers.TryGetValue(type, out var serializer))
                {
                    serializer = new XmlSerializer(type, overrides);
                    _serializers.Add(type, serializer);
                }

                return serializer;
            }
        }
    }
}