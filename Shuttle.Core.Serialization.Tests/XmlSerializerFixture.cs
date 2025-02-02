using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Shuttle.Core.Serialization.Tests
{
    public class XmlSerializerFixture
    {
        [Test]
        public async Task Should_be_able_to_serialize_and_deserialize_a_simple_type_async()
        {
            var original = new SimpleSerializerType();
            var serializer = new XmlSerializer(Options.Create(new XmlSerializerOptions()));

            var stream = await serializer.SerializeAsync(original);

            var xml = await new StreamReader(stream).ReadToEndAsync();

            Assert.That(xml.Contains(original.Id.ToString()), Is.True);

            stream.Position = 0;

            Assert.That(((SimpleSerializerType)await serializer.DeserializeAsync(typeof(SimpleSerializerType), stream)).Id, Is.EqualTo(original.Id));
        }

        [Test]
        public async Task Should_be_able_to_serialize_and_deserialize_a_complex_type_async()
        {
            var complex = new ComplexSerializerType();
            var serializer = new XmlSerializer(Options.Create(new XmlSerializerOptions()));

            serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v1.SomeSerializerType));
            serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v1.AnotherSerializerType));
            serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v2.SomeSerializerType));
            serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v2.AnotherSerializerType));

            var stream = await serializer.SerializeAsync(complex);
            var xml = await new StreamReader(stream).ReadToEndAsync();

            Assert.That(xml.Contains(complex.Id.ToString()), Is.True);

            stream.Position = 0;

            Assert.That(((ComplexSerializerType)await serializer.DeserializeAsync(typeof(ComplexSerializerType), stream)).Id, Is.EqualTo(complex.Id));

            Console.WriteLine(xml);

            var some1 = new v1.SomeSerializerType();
            var some2 = new v2.SomeSerializerType();

            Assert.That(((v1.SomeSerializerType)await serializer.DeserializeAsync(typeof(v1.SomeSerializerType), await serializer.SerializeAsync(some1))).Id, Is.EqualTo(some1.Id));
            Assert.That(((v2.SomeSerializerType)await serializer.DeserializeAsync(typeof(v2.SomeSerializerType), await serializer.SerializeAsync(some2))).Id, Is.EqualTo(some2.Id));
        }
    }
}