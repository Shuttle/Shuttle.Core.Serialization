# Shuttle.Core.Serialization

The `Shuttle.Core.Serialization` package provides a consistent interface for serializing and deserializing objects.

## Installation

```bash
dotnet add package Shuttle.Core.Serialization
```

## Interface

The core of the library is the `ISerializer` interface:

```csharp
public interface ISerializer
{
    string Name { get; }
    Task<Stream> SerializeAsync(object instance, CancellationToken cancellationToken = default);
    Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default);
}
```

The following implementation is provided:

- `JsonSerializer`: uses `System.Text.Json` for serialization.

## Usage

### AddJsonSerializer

To register the `JsonSerializer`, use the `AddJsonSerializer` extension method on `IServiceCollection`:

```csharp
services.AddJsonSerializer(builder => 
{
    builder.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
```

The `builder.Options` is of type [JsonSerializerOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions?view=net-6.0).

### ISerializerService

The `ISerializerService` can be used to manage multiple serializers:

```csharp
public interface ISerializerService
{
    IEnumerable<ISerializer> Serializers { get; }
    ISerializerService Add(ISerializer serializer);
    bool Contains(string name);
    ISerializer Get(string name);
}
```

You can use the `SerializerService` implementation to store and retrieve serializers by their `Name`:

```csharp
var serializerService = new SerializerService();

serializerService.Add(new JsonSerializer(Options.Create(new JsonSerializerOptions())));

if (serializerService.Contains("Json"))
{
    var serializer = serializerService.Get("Json");
}
```

## Methods

### SerializeAsync

```csharp
Task<Stream> SerializeAsync(object instance, CancellationToken cancellationToken = default);
```

Returns the `object` as a `Stream`.

### DeserializeAsync

```csharp
Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default);
```

Deserializes the `Stream` into an `object` of the given `Type`.

### DeserializeAsync&lt;T&gt; (Extension method)

```csharp
Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default);
```

Deserializes the `Stream` into an `object` of the given type `T`.

