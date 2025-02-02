using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization
{
    public class JsonSerializer : ISerializer
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public JsonSerializer(IOptions<JsonSerializerOptions> jsonSerializeOptions)
        {
            _jsonSerializerOptions = Guard.AgainstNull(Guard.AgainstNull(jsonSerializeOptions).Value);
        }

        public async Task<Stream> SerializeAsync(object instance)
        {
            var result = new MemoryStream();

            await System.Text.Json.JsonSerializer.SerializeAsync(result, Guard.AgainstNull(instance), _jsonSerializerOptions).ConfigureAwait(false);

            return result;
        }

        public async Task<object> DeserializeAsync(Type type, Stream stream)
        {
            return await System.Text.Json.JsonSerializer.DeserializeAsync(Guard.AgainstNull(stream), Guard.AgainstNull(type), _jsonSerializerOptions)
                   ?? throw new SerializationException(string.Format(Resources.DeserializationException, type.FullName));
        }

        public string Name => "Json";
    }
}