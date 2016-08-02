using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    class IEnumerableTWriter : IGenericJsonWriter
    {
        public Type Type { get; } = typeof(IEnumerable<>);
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
            public Type Type { get; } = typeof(IEnumerable<T>);

            public InnerWriter()
            {
                var value = typeof(T);

                if (value.IsValueType || value.IsSealed)
                {
                    _writer = GetWrap(value);
                }
            }
            
            private readonly JsonWriterWrapper _writer;

            public void Write(object obj, JsonWriterArgs args)
            {
                if (obj == null)
                {
                    NullWriter.Write(null, args);
                    return;
                }
                var writer = args.Writer;
                var writeValue = _writer != null ? (Action<object, JsonWriterArgs>)_writer.Writer.Write : JsonWriterContainer.Write;

                writer.Write('[');
                var comma = new CommaHelper(writer);
                foreach (var value in (IEnumerable<T>)obj)
                {
                    if (args.IgnoreNullMember)
                    {
                        if (value == null || value is DBNull)
                        {
                            NullWriter.Write(null, args);
                            continue;
                        }
                    }
                    comma.AppendCommaIgnoreFirst();
                    args.WriteCheckLoop(value);
                }

                writer.Write(']');
            }
        }
    }
}
