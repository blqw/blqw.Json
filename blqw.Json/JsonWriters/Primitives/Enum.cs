using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class EnumWriter : IJsonWriter
    {
        public Type Type { get; } = typeof(Enum);
        
        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (Enum)obj;
            JsonWriterContainer.StringWriter.Write(value.ToString(args.EnumToNumber ? "d" : "g"), args);
        }
    }
}