using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization;

public class XmlSerializer : ISerializer, ISerializerRootType
{
    private static readonly object Lock = new();
    private readonly XmlSerializerNamespaces _namespaces = new();

    private readonly Dictionary<Type, XmlAttributeOverrides> _overrides = new();

    private readonly Dictionary<Type, System.Xml.Serialization.XmlSerializer> _serializers = new();
    private readonly XmlDictionaryReaderQuotas _xmlDictionaryReaderQuotas;

    private readonly XmlWriterSettings _xmlWriterSettings;

    public XmlSerializer(IOptions<XmlSerializerOptions> xmlSerializerOptions)
    {
        var options = Guard.AgainstNull(Guard.AgainstNull(xmlSerializerOptions).Value);

        _xmlWriterSettings = new()
        {
            Async = true,
            Encoding = options.Encoding,
            OmitXmlDeclaration = options.OmitXmlDeclaration,
            Indent = options.Indent,
            IndentChars = options.IndentChars,
            NewLineChars = options.NewLineChars,
            NewLineOnAttributes = options.NewLineOnAttributes,
            NewLineHandling = options.NewLineHandling,
            CheckCharacters = options.CheckCharacters,
            ConformanceLevel = options.ConformanceLevel,
            CloseOutput = options.CloseOutput,
            NamespaceHandling = options.NamespaceHandling,
            DoNotEscapeUriAttributes = options.DoNotEscapeUriAttributes,
            WriteEndDocumentOnClose = options.WriteEndDocumentOnClose
        };

        _xmlDictionaryReaderQuotas = new()
        {
            MaxArrayLength = options.MaxArrayLength,
            MaxStringContentLength = options.MaxStringContentLength,
            MaxNameTableCharCount = options.MaxNameTableCharCount,
            MaxBytesPerRead = options.MaxBytesPerRead,
            MaxDepth = options.MaxDepth
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

        lock (Lock)
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

    private System.Xml.Serialization.XmlSerializer GetSerializer(Type type)
    {
        lock (Lock)
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
        lock (Lock)
        {
            if (!_overrides.TryGetValue(root, out var overrides))
            {
                return false;
            }

            return overrides[contained] != null;
        }
    }
}