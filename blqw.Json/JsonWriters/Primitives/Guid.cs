using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class GuidWriter : IJsonWriter
    {
        public Type Type => typeof(Guid);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (Guid) obj;
            var str = value.ToString(args.GuidFormatString);
            if (args.GuidToUpper)
            {
                str = str.ToUpperInvariant();
            }
            args.WriterContainer.GetWriter<string>().Write(str, args);
        }
    }
}