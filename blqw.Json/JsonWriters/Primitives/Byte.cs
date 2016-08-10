using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class ByteWriter : IJsonWriter
    {
        public Type Type => typeof(byte);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (byte) obj;
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