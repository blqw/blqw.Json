using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    internal class ISerializableWrite : List<JsonWriterWrapper>, IMultiJsonWirters, IJsonWriter
    {
        private static readonly IFormatterConverter _Converter = new FormatterConverter();

        private JsonWriterWrapper _wrapper;
        public Type Type => typeof(ISerializable);

        /// <exception cref="SecurityException">调用方没有所要求的权限。</exception>
        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                NullWriter.Write(null, args);
                return;
            }
            var value = (ISerializable) obj;

            var info = new SerializationInfo(obj.GetType(), _Converter);
            value.GetObjectData(info, (new StreamingContext(StreamingContextStates.All, args)));
            if (_wrapper == null)
            {
                _wrapper = GetWrap(typeof(SerializationInfo));
            }
            Debug.Assert(_wrapper != null, "_wrapper != null");
            _wrapper.Writer.Write(info, args);
        }
    }
}