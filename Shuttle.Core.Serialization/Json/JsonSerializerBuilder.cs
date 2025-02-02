using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization
{
    public class JsonSerializerBuilder
    {
        public IServiceCollection Services { get; }

        public JsonSerializerOptions Options
        {
            get => _jsonSerializerOptions;
            set => _jsonSerializerOptions = value ?? throw new ArgumentNullException(nameof(value));
        }

        private JsonSerializerOptions _jsonSerializerOptions = new();

        public JsonSerializerBuilder(IServiceCollection services)
        {
            Services = Guard.AgainstNull(services);
        }
    }
}