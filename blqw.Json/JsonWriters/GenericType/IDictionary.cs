using System;
using System.Collections.Generic;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    internal class IDictionaryTWriter : IGenericJsonWriter
    {
        public Type Type => typeof(IDictionary<,>);

        public IJsonWriter MakeType(Type type)
        {
            foreach (var item in type.GetInterfaces())
            {
                if (item.IsGenericType 
                    && item.IsGenericTypeDefinition == false
                    && item.GetGenericTypeDefinition() == Type)
                {
                    var t = typeof(InnerWriter<,>).MakeGenericType(item.GetGenericArguments());
                    return (IJsonWriter) Activator.CreateInstance(t);
                }
            }
            throw new NotImplementedException();
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            throw new NotImplementedException();
        }

        private class InnerWriter<TKey, TValue> : IJsonWriter
        {
            // ReSharper disable once StaticMemberInGenericType
            private static readonly JsonWriterWrapper _wrapper = GetWrapper();
            private static JsonWriterWrapper GetWrapper()
            {
                var value = typeof(TValue);

                if (value.IsValueType || value.IsSealed)
                {
                    return GetWrap(value);
                }
                return null;
            }

            public Type Type { get; } = typeof(IDictionary<TKey, TValue>);

            public void Write(object obj, JsonWriterArgs args)
            {
                if (obj == null)
                {
                    NullWriter.Write(null, args);
                    return;
                }
                var writer = args.Writer;

                writer.Write('{');
                var comma = new CommaHelper(writer);
                foreach (var item in (IDictionary<TKey, TValue>) obj)
                {
                    var value = item.Value;
                    if (args.IgnoreNullMember)
                    {
                        if (value == null || value is DBNull)
                            continue;
                    }
                    comma.AppendCommaIgnoreFirst();

                    JsonWriterContainer.StringWriter.Write(item.Key as string ?? item.Key.To<string>(), args);
                    writer.Write(':');

                    args.WriteCheckLoop(value, _wrapper?.Writer);
                }

                writer.Write('}');
            }
        }
    }
}