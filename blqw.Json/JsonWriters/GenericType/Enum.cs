using System;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class EnumWriter : IGenericJsonWriter
    {
        public Type Type => typeof(Enum);

        public void Write(object obj, JsonWriterArgs args)
        {
            throw new NotImplementedException();
        }

        public IJsonWriter MakeType(Type type)
        {
            var t = typeof(InnerWriter<>).MakeGenericType(type);
            return (IJsonWriter) Activator.CreateInstance(t);
        }


        private class InnerWriter<T> : IJsonWriter
        {
            private static readonly JsonWriterWrapper _wrapper = GetWrap(Enum.GetUnderlyingType(typeof(T)));


            public Type Type { get; } = typeof(T);

            public void Write(object obj, JsonWriterArgs args)
            {
                var value = (Enum) obj;
                if (args.EnumToNumber)
                {
                    _wrapper.Writer.Write(value, args);
                }
                else
                {
                    JsonWriterContainer.StringWriter.Write(value.ToString("g"), args);
                }
            }
        }
    }
}