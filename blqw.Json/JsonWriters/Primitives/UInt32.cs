using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class UInt32Writer : IJsonWriter
    {
        public Type Type => typeof(uint);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (uint) obj;
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