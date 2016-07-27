﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    class ISerializableWrite : IJsonWriter
    {
        public Type Type { get; } = typeof(ISerializable);
        
        private static readonly IFormatterConverter _Converter = new FormatterConverter();

        private IJsonWriterWrap _wrap = GetWrap(typeof(SerializationInfo));

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;
            var value = (ISerializable)obj;

            var info = new SerializationInfo(obj.GetType(), _Converter);
            value.GetObjectData(info, new StreamingContext());
            _wrap.Writer.Write(info, args);
        }
    }
}