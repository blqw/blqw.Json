using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class Int32Writer : IJsonWriter
    {
        public Type Type => typeof(int);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (int) obj;
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