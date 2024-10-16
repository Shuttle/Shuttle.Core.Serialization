using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization;

public class DefaultSerializer : ISerializer, ISerializerRootType
{
    private static readonly object Padlock = new();
    private readonly XmlSerializerNamespaces _namespaces = new();

    private readonly Dictionary<Type, XmlAttributeOverrides> _overrides = new();

    private readonly Dictionary<Type, XmlSerializer> _serializers = new();
    private readonly XmlDictionaryReaderQuotas _xmlDictionaryReaderQuotas;

    private readonly XmlWriterSettings _xmlWriterSettings;

    public DefaultSerializer()
    {
        _xmlWriterSettings = new()
        {
            Async = true,
            Encoding = Encoding.UTF8,
            OmitXmlDeclaration = true,
            Indent = true
        };

        _xmlDictionaryReaderQuotas = new()
        {
            MaxArrayLength = int.MaxValue,
            MaxStringContentLength = int.MaxValue,
            MaxNameTableCharCount = int.MaxValue
        };

        _namespaces.Add(string.Empty, string.Empty);
    }

    public string Name => "Xml";

    public async Task<Stream> SerializeAsync(object instance)
    {
        Guard.AgainstNull(instance);

        var messageType = instance.GetType();
        var serializer = GetSerializer(messageType);

        var xml = new StringBuilder();

        await using var writer = XmlWriter.Create(xml, _xmlWriterSettings);

        serializer.Serialize(writer, instance, _namespaces);

        await writer.FlushAsync().ConfigureAwait(false);

        var data = Encoding.UTF8.GetBytes(xml.ToString());

        return new MemoryStream(data, 0, data.Length, false, true);
    }

    public async Task<object> DeserializeAsync(Type type, Stream stream)
    {
        Guard.AgainstNull(type);
        Guard.AgainstNull(stream);

        using var copy = new MemoryStream();
        var position = stream.Position;

        stream.Position = 0;

        await stream.CopyToAsync(copy).ConfigureAwait(false);

        stream.Position = position;
        copy.Position = 0;

        using var reader = XmlDictionaryReader.CreateTextReader(copy, Encoding.UTF8, _xmlDictionaryReaderQuotas, null);

        return GetSerializer(type).Deserialize(reader) ?? throw new SerializationException(string.Format(Resources.DeserializationException, type.FullName));
    }

    public void AddSerializerType(Type root, Type contained)
    {
        Guard.AgainstNull(root);
        Guard.AgainstNull(contained);

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
                overrides = new();
                _overrides.Add(root, overrides);
            }

            overrides.Add(contained, new() { XmlRoot = new() { Namespace = contained.Namespace } });

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

    private XmlSerializer GetSerializer(Type type)
    {
        lock (Padlock)
        {
            if (!_overrides.TryGetValue(type, out var overrides))
            {
                overrides = new();
                _overrides.Add(type, overrides);
            }

            if (!_serializers.TryGetValue(type, out var serializer))
            {
                serializer = new(type, overrides);
                _serializers.Add(type, serializer);
            }

            return serializer;
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
}