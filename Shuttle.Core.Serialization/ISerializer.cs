﻿using System;
using System.IO;

namespace Shuttle.Core.Serialization
{
    public interface ISerializer
    {
        Stream Serialize(object instance);
        object Deserialize(Type type, Stream stream);
    }
}