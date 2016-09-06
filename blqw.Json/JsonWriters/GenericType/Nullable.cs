using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class NullableWriter : IGenericJsonWriter
    {
        public Type Type => typeof(Nullable<>);

        public object GetService(Type serviceType)
        {
            var t = typeof(InnerWriter<>).MakeGenericType(serviceType.GetGenericArguments());
            return (IJsonWriter) Activator.CreateInstance(t);
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            throw new NotImplementedException();
        }


        private class InnerWriter<T> : IJsonWriter
            where T : struct
        {
            public Type Type { get; } = typeof(T?);

            public void Write(object obj, JsonWriterArgs args) => args.WriterContainer.GetWriter<T>().Write(obj, args);
        }
    }
}