using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    sealed class NullableWriter : IGenericJsonWriter
    {
        public Type Type { get; } = typeof(Nullable<>);
        
        public IJsonWriter MakeType(Type type)
        {
            foreach (var item in type.GetInterfaces())
            {
                if (item.IsGenericType && item.IsGenericTypeDefinition == false)
                {
                    if (item.GetGenericTypeDefinition() == Type)
                    {
                        var t = typeof(InnerWriter<>).MakeGenericType(item.GetGenericArguments());
                        return (IJsonWriter)Activator.CreateInstance(t);
                    }
                }
            }
            throw new NotImplementedException();
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            throw new NotImplementedException();
        }


        class InnerWriter<T> : IJsonWriter
            where T : struct
        {
            public Type Type { get; } = typeof(T);
            public InnerWriter()
            {
                var value = typeof(T);

                if (value.IsValueType || value.IsSealed)
                {
                    ValueWriter = GetWrap(value);
                }
            }
            public IJsonWriterWrap ValueWriter;

            public void Write(object obj, JsonWriterArgs args)
            {
                ValueWriter.Writer.Write(obj, args);
            }
        }
    }
}