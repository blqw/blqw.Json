using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class SByteWriter : IJsonWriter
    {
        public Type Type => typeof(sbyte);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (sbyte) obj;
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