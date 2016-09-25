using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class UInt16Writer : IJsonWriter
    {
        public Type Type => typeof(ushort);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (ushort) obj;
            var writer = args.Writer;
            if (args.QuotWrapNumber)
            {
                writer.Write('"');
                writer.Write(value);
                writer.Write('"');
            }
            else
            {
                writer.Write(value);
            }
        }
    }
}