using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

            services.AddSingleton(Options.Create(jsonSerializerBuilder.Options));

            return services;
        }

        public static IServiceCollection AddXmlSerializer(this IServiceCollection services, Action<XmlSerializerBuilder>? builder = null)
        {
            Guard.AgainstNull(services);

            var xmlSerializerBuilder = new XmlSerializerBuilder(services);

            builder?.Invoke(xmlSerializerBuilder);

            services.AddSingleton<ISerializer, XmlSerializer>();

            services.AddSingleton(Options.Create(xmlSerializerBuilder.Options));

            return services;
        }
    }
}