# Shuttle.Core.Serialization

## Installation

```bash
dotnet add package Shuttle.Core.Serialization
```

The following implementation of the `ISerializer` interface is used to serialize objects into a `Stream`:

- `JsonSerializer` makes use of the `System.Text.Json` serialization functionality.

## Usage

### JsonSerializer

``` c#
services.AddJsonSerializer(builder => {
	builder.Options = new JsonSerializerOptions 
	{
	};

	// or

	builder.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
```

The `builder.Options` is of type [JsonSerializerOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions?view=net-6.0).

## Methods

### Serialize

``` c#
Task<Stream> SerializeAsync(object instance, CancellationToken cancellationToken = default);
```

Returns the `object` as a `Stream`.

### Deserialize

``` c#
Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default);
```

Deserializes the `Stream` into an `object` of the given type.

