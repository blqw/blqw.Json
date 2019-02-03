using System;
using System.Collections.Generic;


namespace blqw.JsonServices.JsonWriters
{
    internal class FormatProviderWriter : List<JsonWriterWrapper>, IMultiJsonWriters, IJsonWriter
    {
        public Type Type => typeof(IFormatProvider);

        public void Write(object obj, JsonWriterSettings args)
        {
            var obj1 = ((IFormatProvider)obj).GetFormat(typeof(Json));

            if (obj1 != null)
            {
                args.WriteCheckLoop(obj1, null);
            }
            else if (Count == 0)
            {
                args.WriteNull();
            }
            else
            {
                this[0].Writer.Write(obj, args);
            }
        }
    }
}