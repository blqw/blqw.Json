using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class Int64Writer : IJsonWriter
    {
        public Type Type => typeof(long);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (long) obj;
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