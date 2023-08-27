using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Shuttle.Core.Serialization.Tests
{
    public class DefaultSerializerFixture
    {
        [Test]
        public void Should_be_able_to_serialize_and_deserialize_a_simple_type()
        {
            var original = new SimpleSerializerType();
            var serializer = new DefaultSerializer();

            var stream = serializer.Serialize(original);

            var xml = new StreamReader(stream).ReadToEnd();

            Assert.IsTrue(xml.Contains(original.Id.ToString()));

            stream.Position = 0;

            Assert.AreEqual(original.Id, ((SimpleSerializerType)serializer.Deserialize(typeof(SimpleSerializerType), stream)).Id);
        }

        [Test]
        public async Task Should_be_able_to_serialize_and_deserialize_a_simple_type_async()
        {
            var original = new SimpleSerializerType();
            var serializer = new DefaultSerializer();

            var stream = await serializer.SerializeAsync(original);

            var xml = await new StreamReader(stream).ReadToEndAsync();

            Assert.IsTrue(xml.Contains(original.Id.ToString()));

            stream.Position = 0;

            Assert.AreEqual(original.Id, ((SimpleSerializerType)await serializer.DeserializeAsync(typeof(SimpleSerializerType), stream)).Id);
        }

        [Test]
        public void Should_be_able_to_serialize_and_deserialize_a_complex_type()
        {
            var complex = new ComplexSerializerType();
            var serializer = new DefaultSerializer();

            serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v1.SomeSerializerType));
            serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v1.AnotherSerializerType));
            serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v2.SomeSerializerType));
            serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v2.AnotherSerializerType));

            var stream = serializer.Serialize(complex);
            var xml = new StreamReader(stream).ReadToEnd();

            Assert.IsTrue(xml.Contains(complex.Id.ToString()));

            stream.Position = 0;

            Assert.AreEqual(complex.Id, ((ComplexSerializerType)serializer.Deserialize(typeof(ComplexSerializerType), stream)).Id);

            Console.WriteLine(xml);

            var some1 = new v1.SomeSerializerType();
            var some2 = new v2.SomeSerializerType();

            Assert.AreEqual(some1.Id, ((v1.SomeSerializerType)serializer.Deserialize(typeof(v1.SomeSerializerType), serializer.Serialize(some1))).Id);
            Assert.AreEqual(some2.Id, ((v2.SomeSerializerType)serializer.Deserialize(typeof(v2.SomeSerializerType), serializer.Serialize(some2))).Id);
        }

        [Test]
        public async Task Should_be_able_to_serialize_and_deserialize_a_complex_type_async()
        {
            var complex = new ComplexSerializerType();
            var serializer = new DefaultSerializer();

            serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v1.SomeSerializerType));
            serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v1.AnotherSerializerType));
            serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v2.SomeSerializerType));
            serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v2.AnotherSerializerType));

            var stream = await serializer.SerializeAsync(complex);
            var xml = await new StreamReader(stream).ReadToEndAsync();

            Assert.IsTrue(xml.Contains(complex.Id.ToString()));

            stream.Position = 0;

            Assert.AreEqual(complex.Id, ((ComplexSerializerType)await serializer.DeserializeAsync(typeof(ComplexSerializerType), stream)).Id);

            Console.WriteLine(xml);

            var some1 = new v1.SomeSerializerType();
            var some2 = new v2.SomeSerializerType();

            Assert.AreEqual(some1.Id, ((v1.SomeSerializerType)await serializer.DeserializeAsync(typeof(v1.SomeSerializerType), await serializer.SerializeAsync(some1))).Id);
            Assert.AreEqual(some2.Id, ((v2.SomeSerializerType)await serializer.DeserializeAsync(typeof(v2.SomeSerializerType), await serializer.SerializeAsync(some2))).Id);
        }
    }
}