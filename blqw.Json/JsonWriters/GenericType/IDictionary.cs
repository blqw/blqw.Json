using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    class IDictionaryTWriter : IGenericJsonWriter
    {
        public Type Type { get; } = typeof(IDictionary<,>);

        public IJsonWriter MakeType(Type type)
        {
            foreach (var item in type.GetInterfaces())
            {
                if (item.IsGenericType && item.IsGenericTypeDefinition == false)
                {
                    if (item.GetGenericTypeDefinition() == Type)
                    {
                        var t = typeof(InnerWriter<,>).MakeGenericType(item.GetGenericArguments());
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

        class InnerWriter<TKey, TValue> : IJsonWriter
        {
            public InnerWriter()
            {
                var value = typeof(TValue);

                if (value.IsValueType || value.IsSealed)
                {
                    ValueWriter = GetWrap(value);
                }
            }
            public Type Type { get; } = typeof(IDictionary<TKey, TValue>);

            public IJsonWriterWrapper ValueWriter;

            public void Write(object obj, JsonWriterArgs args)
            {
                if (obj == null)
                {
                    JsonWriterContainer.NullWriter.Write(null, args);
                    return;
                }
                var writer = args.Writer;
                var writeValue = ValueWriter != null ? (Action<object, JsonWriterArgs>)ValueWriter.Writer.Write : JsonWriterContainer.Write;

                writer.Write('{');
                var comma = new CommaHelper(writer);
                foreach (var item in (IDictionary<TKey, TValue>)obj)
                {
                    var v = item.Value;
                    if (args.IgnoreNullMember)
                    {
                        if (v == null || v is DBNull)
                            continue;
                    }

                    comma.AppendCommaIgnoreFirst();

                    JsonWriterContainer.StringWriter.Write(item.Key.To<string>(), args);
                    writer.Write(':');
                    writeValue(item.Value, args);
                }

                writer.Write('}');
            }
        }
    }
}
