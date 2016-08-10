using System;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class NullableWriter : IGenericJsonWriter
    {
        public Type Type => typeof(Nullable<>);

        public IJsonWriter MakeType(Type type)
        {
            var t = typeof(InnerWriter<>).MakeGenericType(type.GetGenericArguments());
            return (IJsonWriter) Activator.CreateInstance(t);
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            throw new NotImplementedException();
        }


        private class InnerWriter<T> : IJsonWriter
            where T : struct
        {
            private static readonly JsonWriterWrapper _wrapper = GetWrap(typeof(T));

            public Type Type { get; } = typeof(T?);

            public void Write(object obj, JsonWriterArgs args)
            {
                _wrapper.Writer.Write(obj, args);
            }
        }
    }
}