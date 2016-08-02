using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    internal class FormatProviderWriter : IGenericJsonWriter
    {
        public Type Type { get; } = typeof(IFormatProvider);
        public void Write(object obj, JsonWriterArgs args)
        {
            throw new NotImplementedException();
        }

        public IJsonWriter MakeType(Type type)
        {
            var t = typeof(InnerWriter<>).MakeGenericType(type);
            return (IJsonWriter)Activator.CreateInstance(t);
        }


        private class InnerWriter<T> : IJsonWriter
        {
            private readonly JsonWriterWrapper _wrapper;

            public InnerWriter()
            {
                var value = typeof(T);
                _wrapper = GetWrap(value);
            }

            public Type Type { get; } = typeof(T);

            public void Write(object obj, JsonWriterArgs args)
            {
                var obj1 = (obj as IFormatProvider)?.GetFormat(typeof(Json));
                if (obj1 != null)
                {
                    args.WriteCheckLoop(obj1);
                    return;
                }
                var value = (Enum)obj;
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
