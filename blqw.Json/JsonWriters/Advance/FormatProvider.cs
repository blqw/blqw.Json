using System;
using System.Collections.Generic;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    internal class FormatProviderWriter : List<JsonWriterWrapper>, IMultiJsonWirters, IJsonWriter
    {
        public Type Type => typeof(IFormatProvider);

        public void Write(object obj, JsonWriterArgs args)
        {
            var obj1 = ((IFormatProvider)obj).GetFormat(typeof(Json));

            if (obj1 != null )
            {
                args.WriteCheckLoop(obj1, null);
            }
            else if (Count == 0)
            {
                NullWriter.Write(null, args);
            }
            else
            {
                this[1].Writer.Write(obj, args);
            }
        }
    }
}