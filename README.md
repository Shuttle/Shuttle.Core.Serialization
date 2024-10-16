# Shuttle.Core.Serialization

```
PM> Install-Package Shuttle.Core.Serialization
```

The following implementations of the `ISerializer` interface is used to serialize objects into a `Stream`:

- `XmlSerializer` makes use of the standard .NET XML serialization functionality.
- `JsonSerializer` makes use of the `System.Test.Json` serialization functionality.

## Usage

### XmlSerializer

``` c#
services.AddXmlSerializer(builder => {
	builder.Options = new XmlSerializerOptions 
	{
	};

	// or

	buidler.Options.option = value;
});
```

### JsonSerializer

``` c#
services.AddJsonSerializer(builder => {
	builder.Options = new JsonSerializerOptions 
	{
	};

	// or

	buidler.Options.option = value;
});
```

The `builder.Options` is of type [JsonSerializerOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions?view=net-6.0).

## Methods

### Serialize

``` c#
Task<Stream> SerializeAsync(object message);
```

Returns the message `object` as a `Stream`.

### Deserialize

``` c#
Task<object> DeserializeAsync(Type type, Stream stream);
```

Deserializes the `Stream` into an `object` of the given type.

## ISerializerRootType

The `XmlSerializer` implements the `ISerializerRootType` interface which is an optional interface that serializer implementations can use that allows the developer to specify explicit object types contained within a root type.  

It is recommended that you explicitly register types with the same name, but in different namespaes, that will be serialized within the same root type to avoid any conflicts later down the line.

For instance, the following two types will cause issues when used in the root `Complex` type as they both serialize to the same name and the .Net serializer cannot seem to distinguish the difference:

``` c#
namespace Serializer.v1
{
	public class MovedEvent
	{
		public string Where { get; set; } 
	}
}

namespace Serializer.v2
{
	public class MovedEvent
	{
		public string Where { get; set; } 
	}
}

namespace Serializer
{
	public class Complex
	{
		public v1.MovedEvent { get; set; }
		public v2.MovedEvent { get; set; }
	}
}
```

By explicitly specifying the types the `XmlSerializer` will add a namespace that will cause the types to be correctly identified.

### AddSerializerType

``` c#
void AddSerializerType(Type root, Type contained);
```

Specify the `contained` type that is used within the `root` type.