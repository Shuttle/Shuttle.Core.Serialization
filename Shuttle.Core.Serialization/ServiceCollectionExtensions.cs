using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Shuttle.Core.Serialization;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddJsonSerializer()
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddSingleton<ISerializer, JsonSerializer>();

            return services;
        }

        public IServiceCollection AddJsonSerializer(Action<JsonSerializerOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configureOptions);

            services.AddJsonSerializer();
            services.Configure(configureOptions);

            return services;
        }
    }
}