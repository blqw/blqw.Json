using System;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class GuidWriter : IJsonWriter
    {
        public Type Type => typeof(Guid);

        public void Write(object obj, JsonWriterSettings args)
        {
            var value = (Guid) obj;
            var str = value.ToString(args.GuidFormatString);
            if (args.GuidToUpper)
            {
                str = str.ToUpperInvariant();
            }
            args.Write(str);
        }
    }
}