﻿using System;

namespace Shuttle.Core.Serialization.Tests.v1
{
    public class SomeSerializerType
    {
        public SomeSerializerType()
        {
            Id = Guid.NewGuid();
            AnotherSerializerType = new AnotherSerializerType();
        }

        public Guid Id { get; set; }
        public AnotherSerializerType AnotherSerializerType { get; set; }
    }
}