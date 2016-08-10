using System;
using System.Collections.Generic;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    internal class IEnumerableTWriter : IGenericJsonWriter
    {
        public Type Type => typeof(IEnumerable<>);

        public IJsonWriter MakeType(Type type)
        {
            foreach (var item in type.GetInterfaces())
            {
                if (item.IsGenericType
                    && item.IsGenericTypeDefinition == false
                    && item.GetGenericTypeDefinition() == Type)
                {
                    var t = typeof(InnerWriter<>).MakeGenericType(item.GetGenericArguments());
                    return (IJsonWriter) Activator.CreateInstance(t);
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
            // ReSharper disable once StaticMemberInGenericType
            private static readonly JsonWriterWrapper _wrapper = GetWrapper();

            public Type Type { get; } = typeof(IEnumerable<T>);

            public void Write(object obj, JsonWriterArgs args)
            {
                if (obj == null)
                {
                    NullWriter.Write(null, args);
                    return;
                }
                var writer = args.Writer;

                writer.Write('[');
                var ee = ((IEnumerable<T>) obj).GetEnumerator();
                if (ee.MoveNext())
                {
                    args.WriteCheckLoop(ee.Current, _wrapper?.Writer);
                    while (ee.MoveNext())
                    {
                        args.Writer.Write(',');
                        args.WriteCheckLoop(ee.Current, _wrapper?.Writer);
                    }
                }
                writer.Write(']');
            }

            private static JsonWriterWrapper GetWrapper()
            {
                var value = typeof(T);

                if (value.IsValueType || value.IsSealed)
                {
                    return GetWrap(value);
                }
                return null;
            }
        }
    }
}