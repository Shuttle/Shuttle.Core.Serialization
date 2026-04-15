# Shuttle.Core.Serialization

The `Shuttle.Core.Serialization` package provides a consistent interface for serializing and deserializing objects.

## Installation

```bash
dotnet add package Shuttle.Core.Serialization
```

## `ISerializer` interface

The core of the library is the `ISerializer` interface:

```csharp
public interface ISerializer
{
    string Name { get; }
    Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default);
    Task<Stream> SerializeAsync(object instance, CancellationToken cancellationToken = default);
}
```

The following implementation is provided:

- `JsonSerializer`: uses `System.Text.Json` for serialization.
- `SerializerService`: used to manage multiple `ISerializer` instances.

## Usage

### `AddJsonSerializer`

To register the `JsonSerializer`, use the `AddJsonSerializer` extension method on `IServiceCollection`:

```csharp
services.AddJsonSerializer(options => 
{
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
```

The `options` is of type [JsonSerializerOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions?view=net-6.0).

### `ISerializerService`

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

### `SerializeAsync`

```csharp
Task<Stream> SerializeAsync(object instance, CancellationToken cancellationToken = default);
```

Returns the `object` as a `Stream`.

### `DeserializeAsync`

```csharp
Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default);
```

Deserializes the `Stream` into an `object` of the given `Type`.

### `DeserializeAsync<T>` (Extension method)

```csharp
Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default);
```

Deserializes the `Stream` into an `object` of the given type `T`.

