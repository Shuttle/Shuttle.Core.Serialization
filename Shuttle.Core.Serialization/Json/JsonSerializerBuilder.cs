using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Serialization;

public class JsonSerializerBuilder(IServiceCollection services)
{
    public JsonSerializerOptions Options
    {
        get;
        set => field = value ?? throw new ArgumentNullException(nameof(value));
    } = new();

    public IServiceCollection Services { get; } = Guard.AgainstNull(services);
}