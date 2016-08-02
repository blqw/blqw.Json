using System;
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
        
        private JsonWriterWrapper _wrapper;
        public JsonWriterWrapper Wrapper => _wrapper ?? (_wrapper = GetWrap(typeof(SerializationInfo)));

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                NullWriter.Write(null, args);
                return;
            }
            var value = (ISerializable)obj;

            var info = new SerializationInfo(obj.GetType(), _Converter);
            value.GetObjectData(info, new StreamingContext());
            Wrapper.Writer.Write(info, args);
        }
    }
}
