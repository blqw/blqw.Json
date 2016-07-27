﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    class IListTWriter : IGenericJsonWriter
    {
        public Type Type { get; } = typeof(IList<>);

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
        {
            public Type Type { get; } = typeof(IList<T>);

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
                if (obj == null)
                {
                    JsonWriterContainer.NullWriter.Write(null, args);
                    return;
                }
                var writer = args.Writer;
                var writeValue = ValueWriter != null ? (Action<object, JsonWriterArgs>)ValueWriter.Writer.Write : JsonWriterContainer.Write;
                var list = (IList<T>)obj;

                writer.Write('[');
                var comma = new CommaHelper(writer);
                for (int i = 0,length = list.Count; i < length; i++)
                {
                    var value = list[i];
                    if (args.IgnoreNullMember)
                    {
                        if (value == null || value is DBNull)
                            continue;
                    }

                    comma.AppendCommaIgnoreFirst();

                    writeValue(value, args);
                }

                writer.Write(']');
            }
        }
    }
}