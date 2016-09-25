using System;


namespace blqw.Serializable.JsonWriters
{
    internal sealed class EnumWriter : IGenericJsonWriter
    {
        public Type Type => typeof(Enum);

        public void Write(object obj, JsonWriterArgs args)
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            var t = typeof(InnerWriter<>).MakeGenericType(serviceType);
            return (IJsonWriter) Activator.CreateInstance(t);
        }


        private class InnerWriter<T> : IJsonWriter
        {
            private static readonly Type _UnderlyingType = Enum.GetUnderlyingType(typeof(T));


            public Type Type { get; } = typeof(T);

            public void Write(object obj, JsonWriterArgs args)
            {
                var value = (Enum) obj;
                if (args.EnumToNumber)
                {
                    args.WriterContainer.GetWriter(_UnderlyingType).Write(value, args);
                }
                else
                {
                    args.WriterContainer.GetWriter<string>().Write(value.ToString("g"), args);
                }
            }
        }
    }
}