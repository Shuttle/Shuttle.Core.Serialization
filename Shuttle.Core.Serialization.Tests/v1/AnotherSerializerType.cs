using System;

namespace Shuttle.Core.Serialization.Tests.v1
{
	public class AnotherSerializerType
	{
		public AnotherSerializerType()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }
	}
}