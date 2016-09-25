using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class NullableWriter : IGenericJsonWriter
    {
        public NullableWriter() { }

        private NullableWriter(Type type)
        {
            Type = type;
            UnderlyingType = Nullable.GetUnderlyingType(type);
        }
        public Type Type { get; } = typeof(Nullable<>);

        public Type UnderlyingType { get; }

        JsonWriterContainer.IJsonWriterWrap _wrap;
        private JsonWriterContainer.IJsonWriterWrap Wrap
        {
            get
            {
                return _wrap ?? (_wrap = JsonWriterContainer.GetWrap(UnderlyingType));
            }
        }

        public IJsonWriter MakeType(Type type)
        {
            return new NullableWriter(type);
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            var writer = args.Writer;
            if (obj == null)
            {
                writer.Write("null");
                return;
            }
            if (Wrap == null)
            {
                throw new NotImplementedException($"无法获取 {nameof(Wrap.Writer)}");
            }
            Wrap.Writer.Write(obj, args);
        }
    }
}