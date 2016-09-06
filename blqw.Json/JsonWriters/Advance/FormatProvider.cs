using System;
using System.Collections.Generic;


namespace blqw.Serializable.JsonWriters
{
    internal class FormatProviderWriter : List<JsonWriterWrapper>, IMultiJsonWriters, IJsonWriter
    {
        public Type Type => typeof(IFormatProvider);

        public void Write(object obj, JsonWriterArgs args)
        {
            var obj1 = ((IFormatProvider)obj).GetFormat(typeof(Json));

            if (obj1 != null)
            {
                args.WriteCheckLoop(obj1, null);
            }
            else if (Count == 0)
            {
                args.WriterContainer.GetNullWriter().Write(null, args);
            }
            else
            {
                this[0].Writer.Write(obj, args);
            }
        }
    }
}