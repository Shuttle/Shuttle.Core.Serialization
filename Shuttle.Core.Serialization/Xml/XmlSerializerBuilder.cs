using System;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization
{
    public class XmlSerializerBuilder
    {
        public IServiceCollection Services { get; }

        public XmlSerializerOptions Options
        {
            get => _xmlSerializerOptions;
            set => _xmlSerializerOptions = value ?? throw new ArgumentNullException(nameof(value));
        }

        private XmlSerializerOptions _xmlSerializerOptions = new();

        public XmlSerializerBuilder(IServiceCollection services)
        {
            Services = Guard.AgainstNull(services);
        }
    }
}