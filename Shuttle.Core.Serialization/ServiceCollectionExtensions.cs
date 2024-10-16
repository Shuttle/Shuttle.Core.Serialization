using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonSerializer(this IServiceCollection services, Action<JsonSerializerBuilder>? builder = null)
        {
            Guard.AgainstNull(services);

            var jsonSerializerBuilder = new JsonSerializerBuilder(services);

            builder?.Invoke(jsonSerializerBuilder);

            services.AddSingleton<ISerializer, JsonSerializer>();

            services.AddOptions<JsonSerializerOptions>().Configure(options =>
            {
                options.AllowTrailingCommas = jsonSerializerBuilder.Options.AllowTrailingCommas;
                options.DefaultBufferSize = jsonSerializerBuilder.Options.DefaultBufferSize;
                options.DefaultIgnoreCondition = jsonSerializerBuilder.Options.DefaultIgnoreCondition;
                options.DictionaryKeyPolicy = jsonSerializerBuilder.Options.DictionaryKeyPolicy;
                options.Encoder = jsonSerializerBuilder.Options.Encoder;
                options.IgnoreReadOnlyFields = jsonSerializerBuilder.Options.IgnoreReadOnlyFields;
                options.IncludeFields = jsonSerializerBuilder.Options.IncludeFields;
                options.IgnoreReadOnlyProperties = jsonSerializerBuilder.Options.IgnoreReadOnlyProperties;
                options.MaxDepth = jsonSerializerBuilder.Options.MaxDepth;
                options.NumberHandling = jsonSerializerBuilder.Options.NumberHandling;
                options.PropertyNameCaseInsensitive = jsonSerializerBuilder.Options.PropertyNameCaseInsensitive;
                options.PropertyNamingPolicy = jsonSerializerBuilder.Options.PropertyNamingPolicy;
                options.ReadCommentHandling = jsonSerializerBuilder.Options.ReadCommentHandling;
                options.ReferenceHandler = jsonSerializerBuilder.Options.ReferenceHandler;
                options.UnknownTypeHandling = jsonSerializerBuilder.Options.UnknownTypeHandling;
                options.WriteIndented = jsonSerializerBuilder.Options.WriteIndented;
            });

            return services;
        }

        public static IServiceCollection AddXmlSerializer(this IServiceCollection services, Action<XmlSerializerBuilder>? builder = null)
        {
            Guard.AgainstNull(services);

            var xmlSerializerBuilder = new XmlSerializerBuilder(services);

            builder?.Invoke(xmlSerializerBuilder);

            services.AddSingleton<ISerializer, XmlSerializer>();

            services.AddOptions<XmlSerializerOptions>().Configure(options =>
            {
                options.Indent = xmlSerializerBuilder.Options.Indent;
                options.OmitXmlDeclaration = xmlSerializerBuilder.Options.OmitXmlDeclaration;
                options.Encoding = xmlSerializerBuilder.Options.Encoding;
                options.NewLineHandling = xmlSerializerBuilder.Options.NewLineHandling;
                options.NewLineChars = xmlSerializerBuilder.Options.NewLineChars;
                options.NewLineOnAttributes = xmlSerializerBuilder.Options.NewLineOnAttributes;
                options.CheckCharacters = xmlSerializerBuilder.Options.CheckCharacters;
                options.ConformanceLevel = xmlSerializerBuilder.Options.ConformanceLevel;
                options.CloseOutput = xmlSerializerBuilder.Options.CloseOutput;
                options.IndentChars = xmlSerializerBuilder.Options.IndentChars;
                options.NamespaceHandling = xmlSerializerBuilder.Options.NamespaceHandling;
                options.DoNotEscapeUriAttributes = xmlSerializerBuilder.Options.DoNotEscapeUriAttributes;
                options.WriteEndDocumentOnClose = xmlSerializerBuilder.Options.WriteEndDocumentOnClose;
                options.MaxArrayLength = xmlSerializerBuilder.Options.MaxArrayLength;
                options.MaxStringContentLength = xmlSerializerBuilder.Options.MaxStringContentLength;
                options.MaxNameTableCharCount = xmlSerializerBuilder.Options.MaxNameTableCharCount;
                options.MaxBytesPerRead = xmlSerializerBuilder.Options.MaxBytesPerRead;
                options.MaxDepth = xmlSerializerBuilder.Options.MaxDepth;
            });

            return services;
        }
    }
}