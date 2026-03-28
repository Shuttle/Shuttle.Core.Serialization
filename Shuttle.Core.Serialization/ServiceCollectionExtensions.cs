using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddJsonSerializer(Action<JsonSerializerBuilder>? builder = null)
        {
            Guard.AgainstNull(services);

            var jsonSerializerBuilder = new JsonSerializerBuilder(services);

            builder?.Invoke(jsonSerializerBuilder);

            services.AddSingleton<ISerializer, JsonSerializer>();

            services.Configure<JsonSerializerOptions>(options =>
            {
                options.AllowDuplicateProperties = jsonSerializerBuilder.Options.AllowDuplicateProperties;
                options.AllowOutOfOrderMetadataProperties = jsonSerializerBuilder.Options.AllowOutOfOrderMetadataProperties;
                options.AllowTrailingCommas = jsonSerializerBuilder.Options.AllowTrailingCommas;
                options.PropertyNamingPolicy = jsonSerializerBuilder.Options.PropertyNamingPolicy;
                options.PropertyNameCaseInsensitive = jsonSerializerBuilder.Options.PropertyNameCaseInsensitive;
                options.DictionaryKeyPolicy = jsonSerializerBuilder.Options.DictionaryKeyPolicy;
                options.DefaultIgnoreCondition = jsonSerializerBuilder.Options.DefaultIgnoreCondition;
                options.IgnoreReadOnlyProperties = jsonSerializerBuilder.Options.IgnoreReadOnlyProperties;
                options.IgnoreReadOnlyFields = jsonSerializerBuilder.Options.IgnoreReadOnlyFields;
                options.IncludeFields = jsonSerializerBuilder.Options.IncludeFields;
                options.NumberHandling = jsonSerializerBuilder.Options.NumberHandling;
                options.UnknownTypeHandling = jsonSerializerBuilder.Options.UnknownTypeHandling;
                options.UnmappedMemberHandling = jsonSerializerBuilder.Options.UnmappedMemberHandling;
                options.PreferredObjectCreationHandling = jsonSerializerBuilder.Options.PreferredObjectCreationHandling;
                options.RespectNullableAnnotations = jsonSerializerBuilder.Options.RespectNullableAnnotations;
                options.RespectRequiredConstructorParameters = jsonSerializerBuilder.Options.RespectRequiredConstructorParameters;
                options.ReadCommentHandling = jsonSerializerBuilder.Options.ReadCommentHandling;
                options.MaxDepth = jsonSerializerBuilder.Options.MaxDepth;
                options.WriteIndented = jsonSerializerBuilder.Options.WriteIndented;
                options.IndentCharacter = jsonSerializerBuilder.Options.IndentCharacter;
                options.IndentSize = jsonSerializerBuilder.Options.IndentSize;
                options.NewLine = jsonSerializerBuilder.Options.NewLine;
                options.Encoder = jsonSerializerBuilder.Options.Encoder;
                options.DefaultBufferSize = jsonSerializerBuilder.Options.DefaultBufferSize;
                options.ReferenceHandler = jsonSerializerBuilder.Options.ReferenceHandler;

                foreach (var converter in jsonSerializerBuilder.Options.Converters)
                {
                    options.Converters.Add(converter);
                }

                options.TypeInfoResolver = jsonSerializerBuilder.Options.TypeInfoResolver;
            });

            return services;
        }
    }
}