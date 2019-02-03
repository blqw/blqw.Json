using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;


namespace blqw.JsonServices.JsonWriters
{
    internal class ISerializableWrite : List<JsonWriterWrapper>, IMultiJsonWriters, IJsonWriter
    {
        private static readonly IFormatterConverter _converter = new FormatterConverter();
        
        public Type Type => typeof(ISerializable);

        /// <exception cref="SecurityException">调用方没有所要求的权限。</exception>
        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
                return;
            }
            var value = (ISerializable) obj;
            // TODO:这里需要优化,应该从args里去获取converter
            var info = new SerializationInfo(obj.GetType(), _converter);
            value.GetObjectData(info, (new StreamingContext(StreamingContextStates.All, args)));
            args.Write(info);
        }
    }
}