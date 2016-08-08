using System;
using System.Collections.Generic;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    internal class IListTWriter : IGenericJsonWriter
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
                        return (IJsonWriter) Activator.CreateInstance(t);
                    }
                }
            }
            throw new NotImplementedException();
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            throw new NotImplementedException();
        }

        private class InnerWriter<T> : IJsonWriter
        {
            private readonly JsonWriterWrapper _writer;

            public InnerWriter()
            {
                var value = typeof(T);

                if (value.IsValueType || value.IsSealed)
                {
                    _writer = GetWrap(value);
                }
            }

            public Type Type { get; } = typeof(IList<T>);

            public void Write(object obj, JsonWriterArgs args)
            {
                if (obj == null)
                {
                    NullWriter.Write(null, args);
                    return;
                }
                var writer = args.Writer;
                var list = (IList<T>) obj;
                if (list.Count == 0)
                {
                    writer.Write("[]");
                    return;
                }
                writer.Write('[');
                args.WriteCheckLoop(list[0], _writer?.Writer);

                for (int i = 1, length = list.Count; i < length; i++)
                {
                    args.Writer.Write(',');
                    args.WriteCheckLoop(list[i], _writer?.Writer);
                }

                writer.Write(']');
            }
        }
    }
}