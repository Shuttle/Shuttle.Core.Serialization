using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

            services.AddSingleton(Options.Create(jsonSerializerBuilder.Options));

            return services;
        }
    }
}