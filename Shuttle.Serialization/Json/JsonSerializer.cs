using System.Runtime.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Shuttle.Contract;

namespace Shuttle.Serialization;

public class JsonSerializer(IOptions<JsonSerializerOptions> jsonSerializeOptions) : ISerializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = Guard.AgainstNull(Guard.AgainstNull(jsonSerializeOptions).Value);

    public async Task<Stream> SerializeAsync(object instance, CancellationToken cancellationToken = default)
    {
        var result = new MemoryStream();

        await System.Text.Json.JsonSerializer.SerializeAsync(result, Guard.AgainstNull(instance), _jsonSerializerOptions, cancellationToken).ConfigureAwait(false);

        return result;
    }

    public async Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default)
    {
        return await System.Text.Json.JsonSerializer.DeserializeAsync(Guard.AgainstNull(stream), Guard.AgainstNull(type), _jsonSerializerOptions, cancellationToken)
               ?? throw new SerializationException(string.Format(Resources.DeserializationException, type.FullName));
    }

    public string Name => "Json";
}