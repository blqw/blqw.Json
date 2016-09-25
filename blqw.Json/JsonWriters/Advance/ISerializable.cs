using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;


namespace blqw.Serializable.JsonWriters
{
    internal class ISerializableWrite : List<JsonWriterWrapper>, IMultiJsonWriters, IJsonWriter
    {
        private static readonly IFormatterConverter _Converter = new FormatterConverter();
        
        public Type Type => typeof(ISerializable);

        /// <exception cref="SecurityException">调用方没有所要求的权限。</exception>
        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                args.WriterContainer.GetNullWriter().Write(null, args);
                return;
            }
            var value = (ISerializable) obj;

            var info = new SerializationInfo(obj.GetType(), _Converter);
            value.GetObjectData(info, (new StreamingContext(StreamingContextStates.All, args)));
            var writer = args.WriterContainer.GetWriter<SerializationInfo>();
            writer.Write(info, args);
        }
    }
}