using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class Int16Writer : IJsonWriter
    {
        public Type Type => typeof(short);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (short) obj;
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