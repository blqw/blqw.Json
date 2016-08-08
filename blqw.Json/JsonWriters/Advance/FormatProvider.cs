using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    internal class FormatProviderWriter : List<JsonWriterWrapper>, IMultiJsonWirters, IJsonWriter
    {
        public Type Type { get; } = typeof(IFormatProvider);
        public void Write(object obj, JsonWriterArgs args)
        {
            var obj1 = (obj as IFormatProvider)?.GetFormat(typeof(Json));
            if (obj1 != null)
            {
                args.WriteCheckLoop(obj1,null);
            }
            else if (Count == 0)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
            }
            else
            {
                this[1].Writer.Write(obj, args);
            }
        }

    }
}
