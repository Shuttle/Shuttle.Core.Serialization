using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Shuttle.Core.Serialization.Tests
{
    public class JsonSerializerFixture
    {
        [Test]
        public async Task Should_be_able_to_serialize_and_deserialize_a_simple_type_async()
        {
            var original = new SimpleSerializerType();
            var serializer = new JsonSerializer(Options.Create(new JsonSerializerOptions()));

            var stream = await serializer.SerializeAsync(original);

            stream.Position = 0;

            var json = await new StreamReader(stream).ReadToEndAsync();

            Assert.That(json.Contains(original.Id.ToString()), Is.True);

            stream.Position = 0;

            Assert.That(((SimpleSerializerType)await serializer.DeserializeAsync(typeof(SimpleSerializerType), stream)).Id, Is.EqualTo(original.Id));
        }

        [Test]
        public async Task Should_be_able_to_serialize_and_deserialize_a_complex_type_async()
        {
            var complex = new ComplexSerializerType();
            var serializer = new JsonSerializer(Options.Create(new JsonSerializerOptions()));

            var stream = await serializer.SerializeAsync(complex);

            stream.Position = 0;

            var json = await new StreamReader(stream).ReadToEndAsync();

            Assert.That(json.Contains(complex.Id.ToString()), Is.True);

            stream.Position = 0;

            Assert.That(((ComplexSerializerType)await serializer.DeserializeAsync(typeof(ComplexSerializerType), stream)).Id, Is.EqualTo(complex.Id));

            Console.WriteLine(json);

            var some1 = new v1.SomeSerializerType();
            var some2 = new v2.SomeSerializerType();

            var some1Serialized = await serializer.SerializeAsync(some1);
            var some2Serialized = await serializer.SerializeAsync(some2);

            some1Serialized.Position = 0;
            some2Serialized.Position = 0;

            Assert.That(((v1.SomeSerializerType)await serializer.DeserializeAsync(typeof(v1.SomeSerializerType), some1Serialized)).Id, Is.EqualTo(some1.Id));
            Assert.That(((v2.SomeSerializerType)await serializer.DeserializeAsync(typeof(v2.SomeSerializerType), some2Serialized)).Id, Is.EqualTo(some2.Id));
        }
    }
}